using SimpleJSON;
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
		public enum EVariety
		{
			Grass,
			Dirt,
			Rock,
			Snow,
			Ocean,
		}

		public enum EHeightDelta
		{
			None,
			Increase,
			Decrease,
		}

		[SerializeField]
		public EVariety Variety;

		[SerializeField]
		public EHeightDelta HeightDelta;

		override public void Init()
		{
			center = new Vector2(transform.localPosition.x, transform.localPosition.z);
			height = transform.position.y;
			edgeList = new List<Edge>();
			edgeList.Add(new Edge(center + Vector2.down + Vector2.right));
			edgeList.Add(new Edge(center + Vector2.up + Vector2.right));
			edgeList.Add(new Edge(center + Vector2.up + Vector2.left));
			edgeList.Add(new Edge(center + Vector2.down + Vector2.left));
			justDropped = true;
		}
		override public Edge this[int i]
		{
			get
			{
				i = (i + NumEdges) % NumEdges;
				var edge = edgeList[i];
				edge.PreviousEdge = edgeList[(i - 1 + NumEdges) % NumEdges];
				return edge;
			}
		}

		override public Color Color
		{
			get
			{
				switch (Variety)
				{
					case EVariety.Ocean:
						return new Color(0.0f, 0.3f, 0.9f);
					case EVariety.Grass:
						return new Color(0.2f, 0.9f, 0.4f);
					case EVariety.Dirt:
						return new Color(0.8f, 0.6f, 0.3f);
					case EVariety.Rock:
						return new Color(0.2f, 0.2f, 0.2f);
					case EVariety.Snow:
						return new Color(0.7f, 0.7f, 0.7f);
				}
				return Color.black;
			}
		}

		override public JSONNode ToJson()
		{
			var main = base.ToJson();

			JSONArray nestedEnvironments = new JSONArray();
			foreach (var environment in GetComponentsInChildren<Environment>())
			{
				if (environment == this || environment == null || environment.transform.parent != transform)
				{
					continue;
				}

				nestedEnvironments.Add(environment.ToJson());
			}

			main["variety"] = Variety.ToString();
			main["heightDelta"] = HeightDelta.ToString();
			main["nestedEnvironments"] = nestedEnvironments;

			return main;
		}

		static public bool Validate(GameObject parent)
		{
			var parentEnvironment = parent.GetComponent<Environment>();
			var environments = parent.GetComponentsInChildren<Environment>();
			for (var i = 0; i < environments.Length; ++i)
			{
				if (environments[i] == null)
				{
					continue;
				}

				if (environments[i].gameObject != parent && environments[i].transform.parent.gameObject != parent)
				{
					continue;
				}

				for (var j = i + 1; j < environments.Length; ++j)
				{
					if (environments[j] == null)
					{
						continue;
					}

					if (environments[j].gameObject != parent && environments[j].transform.parent.gameObject != parent)
					{
						continue;
					}

					if (environments[i].CollidesWith(environments[j]))
					{
						Debug.Log(String.Format("Environment \"{0}\" collides with \"{1}\"", environments[i].gameObject.name, environments[j].gameObject.name));
						return false;
					}

					if (environments[i].gameObject != parent && environments[j].gameObject != parent)
					{
						if (environments[i].IsInside(environments[j]))
						{
							Debug.Log(String.Format("Environment \"{0}\" cannot be inside \"{1}\"", environments[i].gameObject.name, environments[j].gameObject.name));
							return false;
						}

						if (environments[j].IsInside(environments[i]))
						{
							Debug.Log(String.Format("Environment \"{0}\" cannot be inside \"{1}\"", environments[j].gameObject.name, environments[i].gameObject.name));
							return false;
						}
					}
				}

				if (environments[i].gameObject != parent)
				{
					if (!Validate(environments[i].gameObject))
					{
						return false;
					}

					if (parentEnvironment != null && !environments[i].IsInside(parentEnvironment))
					{
						Debug.Log(String.Format("Environment \"{0}\" cannot outside parent \"{1}\"", environments[i].gameObject.name, parentEnvironment.gameObject.name));
						return false;
					}
				}
				else
                {
					if (environments[i].IsSelfIntersecting())
					{
						Debug.Log(String.Format("Environment \"{0}\" is self intersecting", environments[i].gameObject.name));
						return false;
					}

					if (!environments[i].IsCCW())
					{
						Debug.Log(String.Format("Environment \"{0}\" is not counter clock wise", environments[i].gameObject.name));
						return false;
					}
                }
			}

			return true;
		}

		bool IsInside(Environment other)
		{
			Vector2 parentPosition = ParentPosition;
			Vector2 OtherParentPosition = other.ParentPosition;
			Bounds otherBounds = other.Bounds;
			Vector2 p1 = this[0].Position + parentPosition;
			Vector2 q1 = new Vector2(p1.x, otherBounds.max.x) + parentPosition;

			int hitCount = 0;
			foreach (var edge in other.edgeList)
            {
				Vector2 p2 = edge.PreviousEdge.Position + OtherParentPosition;
				Vector2 q2 = edge.Position + OtherParentPosition;

				if (DoIntersect(p1, q1, p2, q2, true))
                {
					++hitCount;
                }
			}

			return (hitCount % 2) > 0;
		}

		bool IsCCW()
		{
			Bounds bounds = Bounds;
			float size = Mathf.Max(bounds.size.x, bounds.size.z) * 1.5f;
			Vector2 p1 = this[0].Center - this[0].Normal.normalized * size;
			Vector2 q1 = this[0].Center + this[0].Normal.normalized * size;

			int hitCount = 0;
			float furthestEdgeDistance = 0;
			Vector2 furthestEdgeNormal = Vector2.up;
			foreach (var edge in edgeList)
			{
				Vector2 p2 = edge.PreviousEdge.Position;
				Vector2 q2 = edge.Position;

				if (DoIntersect(p1, q1, p2, q2, true))
				{
					++hitCount;
					var intersectionPoint = IntersectionPoint(p1, q1, p2, q2);
					var distanceToPoint = (intersectionPoint - p1).magnitude;
					if (distanceToPoint > furthestEdgeDistance)
                    {
						furthestEdgeDistance = distanceToPoint;
						furthestEdgeNormal = edge.Normal;
					}
				}
			}

			if ((hitCount % 2) > 0)
            {
				Debug.Log("Ray shouldn't be contained");
			}

			return Vector2.Dot(q1 - p1, furthestEdgeNormal) > 0;
		}
	}
}