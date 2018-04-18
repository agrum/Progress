using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace West
{
	public class ConstellationNode : MonoBehaviour
	{
		private ConstellationNodeSelectable selectable;
		private bool preStartedSelectableNode = false;
		private bool selectableNode = false;
		private List<ConstellationNode> linkedAbilityNodeList = new List<ConstellationNode>();
		private bool isSelected = false;
		private bool started = false;

		public delegate void OnSelectedDelegate(ConstellationNode node, bool selected);
		public event OnSelectedDelegate selectedEvent;

		public void Start()
		{
			started = true;
			selectable = GetComponentInChildren<ConstellationNodeSelectable>();
			
			SelectableNode = preStartedSelectableNode;
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
				if (!started)
				{
					preStartedSelectableNode = value;
					return;
				}

				selectableNode = value;
				selectable.SetSelectable(selectableNode);
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

		private void OnSelected(bool selected)
		{
			selectedEvent(this, selected);
		}
	}
}