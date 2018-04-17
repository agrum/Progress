using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace West
{
	public class ConstellationNodeSelectable : Selectable
	{
		public delegate void OnSelectedDelegate();
		public event OnSelectedDelegate selectedEvent;

		private readonly int enterHash = Animator.StringToHash("Enter");
		private readonly int leaveHash = Animator.StringToHash("Leave");
		private readonly int clickInHash = Animator.StringToHash("ClickIn");
		private readonly int clickOutHash = Animator.StringToHash("ClickOut");
		private readonly int inHash = Animator.StringToHash("In");
		private readonly int outHash = Animator.StringToHash("Out");
		private readonly int unselectedHash = Animator.StringToHash("Unselected");
		private readonly int selectedHash = Animator.StringToHash("Selected");
		private int highlightLayerIndex;
		private int selectLayerIndex;

		private Animator animator_;
		private bool hovered = false;
		private bool selected = false;
		private int highlightNextTrigger = 0;
		private int selectNextTrigger = 0;

		override public void OnPointerEnter(PointerEventData eventData)
		{
			Debug.Log("OnPointerEnter >");
			hovered = true;
			highlightNextTrigger = enterHash;
			Debug.Log("OnPointerEnter <");
		}

		override public void OnPointerExit(PointerEventData eventData)
		{
			Debug.Log("OnPointerExit >");
			hovered = false;
			highlightNextTrigger = leaveHash;
			Debug.Log("OnPointerExit <");
		}

		override public void OnPointerUp(PointerEventData eventData)
		{
			selected = !selected;
			selectNextTrigger = selected ? clickInHash : clickOutHash;
		}

		override protected void Start()
		{
			animator_ = GetComponent<Animator>();
			highlightLayerIndex = animator_.GetLayerIndex("Hover Layer");
			selectLayerIndex = animator_.GetLayerIndex("Click Layer");

			base.Start();
		}

		public void Update()
		{
			if (highlightNextTrigger != 0 && !animator_.IsInTransition(highlightLayerIndex))
			{
				if ((animator.GetCurrentAnimatorStateInfo(highlightLayerIndex).shortNameHash == inHash) != hovered)
				{
					Debug.Log("Update > " + highlightNextTrigger);
					animator_.SetTrigger(highlightNextTrigger);
					highlightNextTrigger = 0;
					Debug.Log("Update <");
				}
			}
			if (selectNextTrigger != 0 && !animator_.IsInTransition(selectLayerIndex))
			{
				if ((animator.GetCurrentAnimatorStateInfo(selectLayerIndex).shortNameHash == selectedHash) != selected)
				{
					animator_.SetTrigger(selectNextTrigger);
					selectNextTrigger = 0;
				}
			}
		}
	}
}