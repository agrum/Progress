using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace West.Asset.World
{
	[System.Serializable]
	public class Environment : LinearFeature
	{
		override public void Init()
		{
			center = new Vector2(transform.localPosition.x, transform.localPosition.z);
			height = transform.position.y;
			edgeList = new List<Edge>();
			edgeList.Add(new Edge(center + Vector2.down + Vector2.right, null));
			edgeList.Add(new Edge(center + Vector2.up + Vector2.right, this[0]));
			edgeList.Add(new Edge(center + Vector2.up + Vector2.left, this[1]));
			edgeList.Add(new Edge(center + Vector2.down + Vector2.left, this[2]));
			justDropped = true;
		}

		override public Edge this[int i]
		{
			get
			{
				Edge edge = edgeList[i % NumEdges];
				if (edge.PreviousEdge == null)
				{
					edge.PreviousEdge = edgeList[NumEdges - 1];
				}
				return edge;
			}
		}
	}
}