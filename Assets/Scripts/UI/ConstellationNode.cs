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
		private List<List<ConstellationNodeLink>> nodeLinkListList = new List<List<ConstellationNodeLink>>();
		private List<ConstellationNodeLink> nodeLinkList = new List<ConstellationNodeLink>();
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
			//new ConstellationNodeLink(this, this);
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

		public List<ConstellationNode> GetNodeInRangeList(int range)
		{
			List<ConstellationNode> nodeInRangeList = new List<ConstellationNode>();

			for (int i = 1; i <= range && i < nodeLinkListList.Count; ++i)
			{
				foreach (var link in nodeLinkListList[i])
				{
					nodeInRangeList.Add(link.Start != this ? link.Start : link.End);
				}
			}

			return nodeInRangeList;
		}

		public ConstellationNodeLink GetLinkTo(ConstellationNode node)
		{
			if (nodeLinkList.Count <= node.Index)
				return null;

			return nodeLinkList[node.Index];
		}

		public void AddLink(ConstellationNodeLink link)
		{
			while (nodeLinkListList.Count <= link.Depth)
				nodeLinkListList.Add(new List<ConstellationNodeLink>());
			while (nodeLinkList.Count <= link.Start.Index || nodeLinkList.Count <= link.End.Index)
				nodeLinkList.Add(null);

			nodeLinkListList[link.Depth].Add(link);
			nodeLinkList[link.Start != this ? link.Start.Index : link.End.Index] = link;
		}

		public void DeepPopulateLinks(int depth)
		{
			while (nodeLinkListList.Count <= depth)
				nodeLinkListList.Add(new List<ConstellationNodeLink>());

			foreach (var nodeLink in nodeLinkListList[depth-1])
			{
				if (nodeLink.Start != this)
					continue;

				foreach (var directNodeLink in nodeLink.End.nodeLinkListList[1])
				{
					//skip lower entries
					if (directNodeLink.Start.Index <= Index || directNodeLink.End.Index <= Index)
						continue;

					ConstellationNode directdEnd = nodeLink.End != directNodeLink.Start ? directNodeLink.Start : directNodeLink.End;
					bool foundInShorterLink = false;
					for (int i = 0; i < depth - 1 && !foundInShorterLink; ++i)
					{
						foreach (var otherNodeLink in nodeLinkListList[i])
						{
							if (this == otherNodeLink.Start && directdEnd == otherNodeLink.End)
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
					new ConstellationNodeLink(nodeLink, directdEnd);
				}
			}

			//if this depth has links, try to deep populate them.
			if (nodeLinkListList[depth].Count > 0)
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