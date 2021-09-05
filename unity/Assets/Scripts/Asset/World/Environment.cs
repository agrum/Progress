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
			edgeList[0].PreviousEdge = edgeList[NumEdges - 1];
			justDropped = true;
		}
		override public Edge this[int i]
		{
			get
			{
				var edge = edgeList[(i + NumEdges) % NumEdges];
				if (edge.PreviousEdge == null)
				{
					edge.PreviousEdge = edgeList[(i - 1 + NumEdges) % NumEdges];
				}
				return edge;
			}
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
			}

			return true;
		}

		bool IsInside(Environment other)
		{
			Bounds otherBounds = other.Bounds;
			Vector2 p1 = this[0].Position;
			Vector2 q1 = new Vector2(p1.x, otherBounds.max.x);

			int hitCount = 0;
			foreach (var edge in other.edgeList)
            {
				Vector2 p2 = edge.PreviousEdge.Position;
				Vector2 q2 = edge.Position;

				if (DoIntersect(p1, q1, p2, q2, true))
                {
					++hitCount;
                }
			}

			return (hitCount % 2) > 0;
		}
	}
}