using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Model
{
	public class ConstellationNode
	{
		public Skill Skill { get; private set; } = null;
		public int Index { get; private set; } = -1;
		public Vector2 Position { get; private set; } = new Vector2Int(0, 0);
		public List<ConstellationNode> KitsNodeList { get; set; } = new List<ConstellationNode>();
		public List<ConstellationNode> ClassNodeList { get; set; } = new List<ConstellationNode>();

		public List<List<ConstellationNodeLink>> abilityNodeLinkListList { get; set; } = new List<List<ConstellationNodeLink>>();
		public List<ConstellationNodeLink> abilityNodeLinkList { get; set; } = new List<ConstellationNodeLink>();

		public ConstellationNode(Skill skill_, int index_, Vector2 position_)
		{
			Skill = skill_;
			Index = index_;
			Position = position_;
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

			foreach (var nodeLink in abilityNodeLinkListList[depth - 1])
			{
				//if (nodeLink.Start != this)
				//	continue;

				bool appendBack = this == nodeLink.Start;
				ConstellationNode deepdEnd = this != nodeLink.Start ? nodeLink.Start : nodeLink.End;
				foreach (var directNodeLink in deepdEnd.abilityNodeLinkListList[1])
				{
					//skip lower entries
					//if (directNodeLink.Start.Index <= Index || directNodeLink.End.Index <= Index)
					//	continue;

					ConstellationNode directdEnd = deepdEnd != directNodeLink.Start ? directNodeLink.Start : directNodeLink.End;
					if (directdEnd == this)
						continue;

					bool foundInShorterLink = false;
					for (int i = 0; i < depth - 1 && !foundInShorterLink; ++i)
					{
						foreach (var otherNodeLink in abilityNodeLinkListList[i])
						{
							if ((this == otherNodeLink.Start && directdEnd == otherNodeLink.End) || (this == otherNodeLink.End && directdEnd == otherNodeLink.Start))
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
					new ConstellationNodeLink(nodeLink, directdEnd, appendBack);
				}
			}

			//if this depth has links, try to deep populate them.
			if (abilityNodeLinkListList[depth].Count > 0)
				DeepPopulateLinks(depth + 1);
		}
	}
}
