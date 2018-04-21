using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace West
{
	public class ConstellationNode : Selectable
	{
		public delegate void OnSelectedDelegate(ConstellationNode node, bool selected);
		public event OnSelectedDelegate selectedEvent;

		private readonly int enterHash = Animator.StringToHash("Enter");
		private readonly int leaveHash = Animator.StringToHash("Leave");
		private readonly int clickInHash = Animator.StringToHash("ClickIn");
		private readonly int clickOutHash = Animator.StringToHash("ClickOut");
		private readonly int inHash = Animator.StringToHash("In");
		private readonly int selectedHash = Animator.StringToHash("Selected");
		private readonly int isUnselectableHash = Animator.StringToHash("IsUnselectable");
		private readonly int isSelectableHash = Animator.StringToHash("IsSelectable");
		private int highlightLayerIndex;
		private int selectLayerIndex;
		private List<ConstellationNode> linkedAbilityNodeList = new List<ConstellationNode>();
		private bool preStartedSelectableNode = false;
		private bool selectableNode = false;
		private bool started = false;
		
		private bool hovered = false;
		private bool selected = false;
		private bool isSelectable = false;
		private int highlightNextTrigger = 0;
		private int selectNextTrigger = 0;

		override protected void Start()
		{
			started = true;

			SelectableNode = preStartedSelectableNode;

			//animator_ = GetComponent<Animator>();
			highlightLayerIndex = animator.GetLayerIndex("Hover Layer");
			selectLayerIndex = animator.GetLayerIndex("Click Layer");

			base.Start();
		}

		public void Update()
		{
			if (highlightNextTrigger != 0 && !animator.IsInTransition(highlightLayerIndex))
			{
				if ((animator.GetCurrentAnimatorStateInfo(highlightLayerIndex).shortNameHash == inHash) != hovered)
				{
					animator.SetTrigger(highlightNextTrigger);
					highlightNextTrigger = 0;
				}
			}
			if (selectNextTrigger != 0 && !animator.IsInTransition(selectLayerIndex))
			{
				if ((animator.GetCurrentAnimatorStateInfo(selectLayerIndex).shortNameHash == selectedHash) != selected)
				{
					animator.SetTrigger(selectNextTrigger);
					selectNextTrigger = 0;
				}
			}
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
				SetSelectable(selectableNode);
			}
		}

		public static void Link(ConstellationNode a, ConstellationNode b)
		{
			if (!a.linkedAbilityNodeList.Contains(b) && !b.linkedAbilityNodeList.Contains(a))
			{
				a.linkedAbilityNodeList.Add(b);
				b.linkedAbilityNodeList.Add(a);
			}
		}

		override public void OnPointerEnter(PointerEventData eventData)
		{
			hovered = true;
			highlightNextTrigger = enterHash;
		}

		override public void OnPointerExit(PointerEventData eventData)
		{
			hovered = false;
			highlightNextTrigger = leaveHash;
		}

		override public void OnPointerUp(PointerEventData eventData)
		{
			if (isSelectable)
			{
				selected = !selected;
				selectedEvent(this, selected);
				selectNextTrigger = selected ? clickInHash : clickOutHash;
			}
		}

		public void SetSelectable(bool selectable)
		{
			isSelectable = selectable;
			if (isSelectable)
				animator.SetTrigger(isSelectableHash);
			else
				animator.SetTrigger(isUnselectableHash);
		}
	}
}