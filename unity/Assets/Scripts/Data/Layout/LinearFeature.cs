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
		public string Name;
		public List<Edge> EdgeList = new List<Edge>();

		public LinearFeature(JSONNode node)
		{
			Name = node["name"];
			foreach (var edge in node["vertices"].AsArray)
			{
				EdgeList.Add(new Edge(edge));
			}
		}

		public LinearFeature(string name_, Vector2 center_, Vector2 size_)
        {
			var halfSize = size_ * 0.5f;
			Name = name_;
			EdgeList.Add(new Edge(center_ + new Vector2(halfSize.x, halfSize.y)));
			EdgeList.Add(new Edge(center_ + new Vector2(-halfSize.x, halfSize.y)));
			EdgeList.Add(new Edge(center_ + new Vector2(-halfSize.x, -halfSize.y)));
			EdgeList.Add(new Edge(center_ + new Vector2(halfSize.x, -halfSize.y)));
		}

		public LinearFeature(LinearFeature other_)
		{
			Name = other_.Name;
			foreach (var edge in other_.EdgeList)
			{
				EdgeList.Add(new Edge(edge));
			}
		}

		virtual public Edge this[int i]
		{
			get
			{
				i = (i + NumEdges) % NumEdges;
				var edge = EdgeList[i];
				if (i > 0)
                {
					edge.PreviousEdge = EdgeList[i-1];
				}
				return edge;
			}
		}

		public int NumEdges
		{
			get { return EdgeList.Count; }
		}
	}
}