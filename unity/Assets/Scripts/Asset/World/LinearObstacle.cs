using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace West.Asset.World
{
	[System.Serializable]
	public class LinearObstacle : LinearFeature
	{
		override public void Init()
		{
			center = new Vector2(transform.localPosition.x, transform.localPosition.z);
			height = transform.position.y;
			edgeList = new List<Edge>();
			edgeList.Add(new Edge(center + (Vector2.down + Vector2.right) * 2.0f, null));
			edgeList.Add(new Edge(center, this[0]));
			edgeList.Add(new Edge(center + (Vector2.up + Vector2.left) * 2.0f, this[1]));
			justDropped = true;
		}

		override public Color Color
		{
			get
			{
				return Color.black;
			}
		}

		public bool Validate()
		{
			if (IsSelfIntersecting())
			{
				Debug.Log(String.Format("Linear obstacle \"{0}\" is self intersecting", gameObject.name));
				return false;
			}
			return true;
		}
	}
}