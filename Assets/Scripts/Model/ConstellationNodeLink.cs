using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Model
{
	public class ConstellationNodeLink
	{
		public List<ConstellationNode> nodeList;

		public ConstellationNodeLink(ConstellationNode a, ConstellationNode b)
		{
			nodeList = new List<ConstellationNode>();
			nodeList.Add(a);
			nodeList.Add(b);
			a.AddLink(this);
			b.AddLink(this);
		}

		public ConstellationNodeLink(ConstellationNodeLink link, ConstellationNode extensionNode, bool appendBack)
		{
			nodeList = new List<ConstellationNode>();
			if (appendBack)
			{
				nodeList.AddRange(link.nodeList);
				nodeList.Add(extensionNode);
			}
			else
			{
				nodeList.Add(extensionNode);
				nodeList.AddRange(link.nodeList);
			}

			this.Start.AddLink(this);
			this.End.AddLink(this);
		}

		public int Depth { get { return nodeList.Count - 1; } }
		public ConstellationNode Start { get { return nodeList[0]; } }
		public ConstellationNode End { get { return nodeList[Depth]; } }

		public static List<List<ConstellationNodeLink>> GetRouteList(List<int> remainIndexList, ConstellationNodeLink[,] linkTable, int sourceIndex)
		{
			List<List<ConstellationNodeLink>> routeList = new List<List<ConstellationNodeLink>>();

			if (remainIndexList.Count == 0)
				return routeList;

			for (int i = 0; i < remainIndexList.Count; ++i)
			{
				Model.ConstellationNodeLink link = linkTable[sourceIndex, remainIndexList[i]];
				List<List<ConstellationNodeLink>> subRouteList = new List<List<ConstellationNodeLink>>();

				if (remainIndexList.Count > 1)
				{
					List<int> subRemainIndexList = new List<int>(remainIndexList);
					subRemainIndexList.RemoveAt(i);

					subRouteList.AddRange(GetRouteList(subRemainIndexList, linkTable, sourceIndex));
					subRouteList.AddRange(GetRouteList(subRemainIndexList, linkTable, remainIndexList[i]));
					for (int j = 0; j < subRouteList.Count; ++j)
						subRouteList[j].Add(link);
				}
				else
				{
					subRouteList.Add(new List<ConstellationNodeLink>());
					subRouteList[0].Add(link);
				}

				routeList.AddRange(subRouteList);
			}

			return routeList;
		}
	}
}