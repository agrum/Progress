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

		public int Index { get; set; } = -1;

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
		private List<List<ConstellationNode>> linkedAbilityNodeList = new List<List<ConstellationNode>>();
		private bool preStartedSelectableNode = false;
		private bool selectableNode = false;
		private bool started = false;
		
		private bool hovered = false;
		private bool selected = false;
		private bool isSelectable = false;
		private int highlightNextTrigger = 0;
		private int selectNextTrigger = 0;

		public ConstellationNode()
		{
			linkedAbilityNodeList.Add(new List<ConstellationNode>());
			linkedAbilityNodeList[0].Add(this);
		}

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

		public static void Link(ConstellationNode a, ConstellationNode b, int depth)
		{
			while (a.linkedAbilityNodeList.Count <= depth)
				a.linkedAbilityNodeList.Add(new List<ConstellationNode>());
			while (b.linkedAbilityNodeList.Count <= depth)
				b.linkedAbilityNodeList.Add(new List<ConstellationNode>());

			if (!a.linkedAbilityNodeList[depth].Contains(b) && !b.linkedAbilityNodeList[depth].Contains(a))
			{
				a.linkedAbilityNodeList[depth].Add(b);
				b.linkedAbilityNodeList[depth].Add(a);
			}
		}

		public static int LinkDepth(ConstellationNode a, ConstellationNode b)
		{
			for (int i = 0; i < a.linkedAbilityNodeList.Count; ++i)
			{
				foreach (var linkedNode in a.linkedAbilityNodeList[i])
				{
					if (linkedNode == b)
						return i;
				}
			}

			return int.MaxValue;
		}

		public List<int> LinkedNodeIndexList(int maxDepth)
		{
			var linkedNodeList = new List<ConstellationNode>();
			var linkedNodeIndexList = new List<int>();
			for (int i = 0; i <= maxDepth && i < linkedAbilityNodeList.Count; ++i)
			{
				linkedNodeList.AddRange(linkedAbilityNodeList[i]);
			}

			foreach (var node in linkedNodeList)
				linkedNodeIndexList.Add(node.Index);

			return linkedNodeIndexList;
		}

		public void DeepPopulateLinks(int depth)
		{
			while (linkedAbilityNodeList.Count <= depth)
				linkedAbilityNodeList.Add(new List<ConstellationNode>());

			foreach (var node in linkedAbilityNodeList[depth-1])
			{
				foreach (var deepLinkedNode in node.linkedAbilityNodeList[1])
				{
					//ignore index lower than us
					if (deepLinkedNode.Index < Index)
						continue;

					bool foundInShorterLink = false;
					for (int i = 0; i < linkedAbilityNodeList.Count - 2 && !foundInShorterLink; ++i)
					{
						foreach (var otherLinkedNode in linkedAbilityNodeList[i])
						{
							if (otherLinkedNode == deepLinkedNode)
							{
								foundInShorterLink = true;
								break;
							}
						}
					}

					//ignore if already linkied with shorter link
					if (foundInShorterLink)
						continue;

					//add to this depth
					Link(this, deepLinkedNode, depth);
				}
			}

			//if this depth has links, try to deep populate them.
			if (linkedAbilityNodeList[depth].Count > 0)
				DeepPopulateLinks(depth+1);
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
			{
				animator.SetTrigger(isSelectableHash);
			}
			else
			{
				selected = false;
				animator.ResetTrigger(isSelectableHash);
				animator.SetTrigger(isUnselectableHash);
			}
		}
	}
}