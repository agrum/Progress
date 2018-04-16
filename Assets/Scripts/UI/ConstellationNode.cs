using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace West
{
	public class ConstellationNode : MonoBehaviour
	{
		private ConstellationNodeSelectable selectable;
		private Animator pulse;
		private bool selectableNode = false;
		private List<ConstellationNode> linkedAbilityNodeList = new List<ConstellationNode>();
		private bool isSelected = false;

		public delegate void OnSelectedDelegate(ConstellationNode node);
		public event OnSelectedDelegate selectedEvent;

		public void Setup()
		{
			selectable = GetComponentInChildren<ConstellationNodeSelectable>();
			pulse = GetComponentsInChildren<Animator>()[1];
			
			SelectableNode = false;
			selectable.selectedEvent += OnSelected;
		}

		public void Update()
		{

		}

		public bool SelectableNode
		{
			get
			{
				return selectableNode;
			}
			set
			{
				selectableNode = value;
				if(selectableNode)
				{
					pulse.gameObject.SetActive(true);
					pulse.enabled = true;
					pulse.Play("Play");
				}
				else
				{
					pulse.gameObject.SetActive(false);
					pulse.enabled = false; 
				}
			}
		}

		public void SelectNode()
		{
			isSelected = !isSelected;
		}

		public static void Link(ConstellationNode a, ConstellationNode b)
		{
			if (!a.linkedAbilityNodeList.Contains(b) && !b.linkedAbilityNodeList.Contains(a))
			{
				a.linkedAbilityNodeList.Add(b);
				b.linkedAbilityNodeList.Add(a);
			}
		}

		private void OnSelected()
		{
			selectedEvent(this);
		}
	}
}