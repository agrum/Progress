using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace West
{
	public class ConstellationNode : MonoBehaviour
	{
		private Selectable selectable;
		private Animator pulse;
		private Image[] imageArray;
		private bool selectableNode = false;

		public void Setup()
		{
			selectable = GetComponentInChildren<Selectable>();
			pulse = GetComponentsInChildren<Animator>()[1];
			imageArray = GetComponentsInChildren<Image>();
			
			SelectableNode = false;
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
	}
}