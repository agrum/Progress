using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleJSON;

namespace West
{
	public class ConstellationNode : Selectable
	{
		public enum ConstellationNodeType
		{
			Ability,
			Kit,
			Class,
			None
		}

		public delegate void OnSelectedDelegate(ConstellationNode node, bool selected);
		public event OnSelectedDelegate selectedEvent;

		public ConstellationNodeType Type { get; set; } = ConstellationNodeType.None;
		public Material Mat { get; set; }
		public int Index { get; set; } = -1;
		public JSONNode Json { get; private set; } = null;
		private string uuid = "";
		
		public List<ConstellationNode> KitsNodeList { get; set; } = new List<ConstellationNode>();
		public List<ConstellationNode> ClassNodeList { get; set; } = new List<ConstellationNode>();

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
		private List<List<ConstellationNodeLink>> abilityNodeLinkListList = new List<List<ConstellationNodeLink>>();
		private List<ConstellationNodeLink> abilityNodeLinkList = new List<ConstellationNodeLink>();
		private bool preStartedSelectableNode = false;
		private bool selectableNode = false;
		private bool started = false;
		
		private bool hovered = false;
		private bool selected = false;
		private bool isSelectable = false;
		private int highlightNextTrigger = 0;
		private int selectNextTrigger = 0;
		
		public string Uuid
		{
			get
			{
				return uuid;
			}
			set
			{
				uuid = value;
				string lowerCaseKey = "";
				string UpperCamelCaseKey = "";
				switch(Type)
				{
					case ConstellationNodeType.Ability:
						lowerCaseKey = "abilities";
						UpperCamelCaseKey = "Abilities";
						break;
					case ConstellationNodeType.Class:
						lowerCaseKey = "classes";
						UpperCamelCaseKey = "Classes";
						break;
					case ConstellationNodeType.Kit:
						lowerCaseKey = "kits";
						UpperCamelCaseKey = "Kits";
						break;
				}

				JSONArray abilityArray = App.Model[lowerCaseKey].AsArray;
				foreach (var node in abilityArray)
				{
					string nodeId1 = node.Value["_id"];
					string nodeId2 = uuid;
					bool nodeId3 = nodeId1 == nodeId2;
					if (node.Value["_id"] == uuid)
					{
						Json = node.Value;
					}
				}

				Transform childTranform = gameObject.transform.Find("GameObject");
				Image stroke = childTranform.Find("HexagonStroke").GetComponent<Image>();
				Image pulse = childTranform.Find("Pulse").GetComponent<Image>();
				Image fill = childTranform.Find("HexagonFill").GetComponent<Image>();
				Image icon = childTranform.Find("Icon").GetComponent<Image>();
				Image iconWhite = childTranform.Find("White").Find("Icon").GetComponent<Image>();
				string path = "Icons/" + UpperCamelCaseKey + "/" + Json["name"];
				Object prefabObject = Resources.Load(path) ;
				Texture2D texture = prefabObject as Texture2D;
				Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1.0f);

				stroke.material = Mat;
				pulse.material = Mat;
				fill.material = Mat;
				icon.material = Mat;
				icon.overrideSprite = sprite;
				iconWhite.overrideSprite = sprite;
			}
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

			for (int i = 1; i <= range && i < abilityNodeLinkListList.Count; ++i)
			{
				foreach (var link in abilityNodeLinkListList[i])
				{
					nodeInRangeList.Add(link.Start != this ? link.Start : link.End);
				}
			}

			return nodeInRangeList;
		}

		public ConstellationNodeLink GetLinkTo(ConstellationNode node)
		{
			if (abilityNodeLinkList.Count <= node.Index)
				return null;

			return abilityNodeLinkList[node.Index];
		}

		public void AddLink(ConstellationNodeLink link)
		{
			while (abilityNodeLinkListList.Count <= link.Depth)
				abilityNodeLinkListList.Add(new List<ConstellationNodeLink>());
			while (abilityNodeLinkList.Count <= link.Start.Index || abilityNodeLinkList.Count <= link.End.Index)
				abilityNodeLinkList.Add(null);

			abilityNodeLinkListList[link.Depth].Add(link);
			abilityNodeLinkList[link.Start != this ? link.Start.Index : link.End.Index] = link;
		}

		public void DeepPopulateLinks(int depth)
		{
			while (abilityNodeLinkListList.Count <= depth)
				abilityNodeLinkListList.Add(new List<ConstellationNodeLink>());

			foreach (var nodeLink in abilityNodeLinkListList[depth-1])
			{
				if (nodeLink.Start != this)
					continue;

				foreach (var directNodeLink in nodeLink.End.abilityNodeLinkListList[1])
				{
					//skip lower entries
					if (directNodeLink.Start.Index <= Index || directNodeLink.End.Index <= Index)
						continue;

					ConstellationNode directdEnd = nodeLink.End != directNodeLink.Start ? directNodeLink.Start : directNodeLink.End;
					bool foundInShorterLink = false;
					for (int i = 0; i < depth - 1 && !foundInShorterLink; ++i)
					{
						foreach (var otherNodeLink in abilityNodeLinkListList[i])
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
			if (abilityNodeLinkListList[depth].Count > 0)
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
				animator.ResetTrigger(isSelectableHash);
				selectNextTrigger = selected ? clickInHash : clickOutHash;
			}
		}

		public void SetSelectable(bool selectable)
		{
			isSelectable = selectable;
			if (isSelectable)
			{
				animator.ResetTrigger(isSelectableHash);
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