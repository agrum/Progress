using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Data.Layout
{
	public class LinearFeature
	{
		public string name;
		public List<Edge> edgeList;

		public LinearFeature(JSONNode node)
		{
			name = node["name"];
			foreach (var edge in node["vertices"].AsArray)
            {
				edgeList.Add(new Edge(edge));
            }
		}

		virtual public Edge this[int i]
		{
			get
			{
				i = (i + NumEdges) % NumEdges;
				var edge = edgeList[i];
				if (i > 0)
                {
					edge.PreviousEdge = edgeList[i-1];
				}
				return edge;
			}
		}

		public int NumEdges
		{
			get { return edgeList.Count; }
		}
	}
}