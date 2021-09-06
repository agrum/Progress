using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Assets.Scripts.Data.Layout.Environment;

namespace West.Asset.World
{
	[System.Serializable]
	public class LinearObstacle : LinearFeature
	{
		[SerializeField]
		public EVariety Variety;

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
				switch (Variety)
				{
					case EVariety.River:
						return new Color(0.0f, 0.3f, 0.9f);
					case EVariety.Fissure:
						return new Color(0.8f, 0.6f, 0.3f);
					case EVariety.Wall:
						return new Color(0.2f, 0.2f, 0.2f);
				}
				return Color.black;
			}
		}

		override public JSONNode ToJson()
		{
			var main = base.ToJson();

			main["variety"] = Variety.ToString();

			return main;
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