using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace West
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

		public ConstellationNodeLink(ConstellationNodeLink link, ConstellationNode extensionNode)
		{
			nodeList = new List<ConstellationNode>(link.nodeList);
			nodeList.Add(extensionNode);
			this.Start.AddLink(this);
			this.End.AddLink(this);
		}

		public int Depth
		{
			get
			{
				return nodeList.Count-1;
			}
		}

		public ConstellationNode Start
		{
			get
			{
				return nodeList[0];
			}
		}

		public ConstellationNode End
		{
			get
			{
				return nodeList[Depth];
			}
		}
	}
}