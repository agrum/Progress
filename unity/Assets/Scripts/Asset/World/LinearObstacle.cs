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
		public enum EType
		{
			River,
			Wall,
			Fissure,
		}

		[SerializeField]
		public EType Type;

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
				switch (Type)
				{
					case EType.River:
						return new Color(0.0f, 0.3f, 0.9f);
					case EType.Fissure:
						return new Color(0.8f, 0.6f, 0.3f);
					case EType.Wall:
						return new Color(0.2f, 0.2f, 0.2f);
				}
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