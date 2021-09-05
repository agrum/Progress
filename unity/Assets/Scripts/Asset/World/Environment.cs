using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace West.Asset.World
{
	[System.Serializable]
	public class Environment : MonoBehaviour
	{
		[SerializeField]
		public string name;

		[SerializeField, HideInInspector]
		public List<EnvironmentEdge> edgeList;

		[SerializeField, HideInInspector]
		public Vector2 center;

		[SerializeField, HideInInspector]
		public float height;

		[SerializeField, HideInInspector]
		public bool justDropped;

		private CircleCollider2D circleCollider;
		private PolygonCollider2D polyCollider;

		public void Init()
		{
			center = new Vector2(transform.localPosition.x, transform.localPosition.z);
			height = 0;
			edgeList = new List<EnvironmentEdge>();
			edgeList.Add(new EnvironmentEdge(center + Vector2.down + Vector2.right, null));
			edgeList.Add(new EnvironmentEdge(center + Vector2.up + Vector2.right, this[0]));
			edgeList.Add(new EnvironmentEdge(center + Vector2.up + Vector2.left, this[1]));
			edgeList.Add(new EnvironmentEdge(center + Vector2.down + Vector2.left, this[2]));
			justDropped = true;
		}

		public EnvironmentEdge this[int i]
		{
			get
			{
				EnvironmentEdge edge = edgeList[i % NumEdges];
				edge.NextEdge = edgeList[(i + 1) % NumEdges];
				return edge;
			}
		}

		public void Merge(int i)
		{
			if (NumEdges > 2)
			{
				this[i + 1].Position = (this[i].Position + this[i + 1].Position) / 2.0f;
				edgeList.RemoveAt(i % NumEdges);
			}
		}

		public void Split(int i)
		{
			edgeList.Insert((i % NumEdges) + 1, new EnvironmentEdge((this[i + 1].Position + this[i].Position) / 2.0f, this[i + 1]));
		}

		public int NumEdges
		{
			get { return edgeList.Count; }
		}

		public void AddSegment(Vector2 anchorPoint)
		{
			float bestSqrDistance = Mathf.Infinity;
			int bestCandidateIndex = 0;
			if (NumEdges == 2)
			{
				if (Vector2.Dot(this[1].Normal.normalized, anchorPoint - this[1].Center) > 0)
					bestCandidateIndex = 1;
			}
			else
			{
				for (int i = 0; i < edgeList.Count; ++i)
				{
					float candidateDistance = (anchorPoint - (this[i].Position + this[i + 1].Position) / 2.0f).sqrMagnitude;
					if (candidateDistance < bestSqrDistance)
					{
						bestSqrDistance = candidateDistance;
						bestCandidateIndex = i;
					}
				}
			}

			Split(bestCandidateIndex);
			this[bestCandidateIndex + 1].Position = anchorPoint;
		}

		static public bool Validate(GameObject parent)
		{
			var environments = parent.GetComponentsInChildren<Asset.World.Environment>();
			for (var i = 0; i < environments.Length; ++i)
			{
				if (environments[i] == null)
				{
					continue;
				}

				for (var j = i + 1; j < environments.Length; ++j)
				{
					if (environments[j] == null)
					{
						continue;
					}

					if (environments[i].CollidesWith(environments[j]))
					{
						Debug.Log(String.Format("Environment \"{0}\" collides with \"{1}\"", environments[i].gameObject.name, environments[j].gameObject.name));
						return false;
					}
				}

				if (environments[i].gameObject != parent && !Validate(environments[i].gameObject))
				{
					return false;
				}
			}
			return true;
		}

		public bool CollidesWith(Environment other)
		{
			Vector2 parentPosition = new Vector2(transform.position.x - transform.localPosition.x, transform.position.z - transform.localPosition.z);
			Vector2 OtherParentPosition = new Vector2(other.transform.position.x - other.transform.localPosition.x, other.transform.position.z - other.transform.localPosition.z);
			for (int i = 0; i < NumEdges; ++i)
			{
				Vector2 p1 = parentPosition + this[i].Position;
				Vector2 p2 = parentPosition + this[i + 1].Position;
				for (int j = 0; j < other.NumEdges; ++j)
				{
					Vector2 p3 = OtherParentPosition + other[j].Position;
					Vector2 p4 = OtherParentPosition + other[j + 1].Position;
					if (DoIntersect(p1, p2, p3, p4, true))
                    {
						return true;
                    }
				}
			}
			return false;
		}

		// Given three points p, q, r, the function checks if
		// point q lies on line segment 'pr'
		bool OnSegment(Vector2 p, Vector2 q, Vector2 r)
		{
			if (q.x <= Mathf.Max(p.x, r.x) && q.x >= Mathf.Min(p.x, r.x) &&
				q.y <= Mathf.Max(p.y, r.y) && q.y >= Mathf.Min(p.y, r.y))
				return true;

			return false;
		}

		// To find orientation of ordered triplet (p, q, r).
		// The function returns following values
		// 0 --> p, q and r are colinear
		// 1 --> Clockwise
		// 2 --> Counterclockwise
		int Orientation(Vector2 p, Vector2 q, Vector2 r)
		{
			// See https://www.geeksforgeeks.org/orientation-3-ordered-edgeList/
			// for details of below formula.
			float val = (q.y - p.y) * (r.x - q.x) -
						(q.x - p.x) * (r.y - q.y);

			if (val == 0) return 0;  // colinear

			return (val > 0) ? 1 : 2; // clock or counterclock wise
		}

		// The main function that returns true if line segment 'p1q1'
		// and 'p2q2' intersect.
		bool DoIntersect(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2, bool checkForColinear)
		{
			// Find the four orientations needed for general and
			// special cases
			int o1 = Orientation(p1, q1, p2);
			int o2 = Orientation(p1, q1, q2);
			int o3 = Orientation(p2, q2, p1);
			int o4 = Orientation(p2, q2, q1);

			// General case
			if (o1 != o2 && o3 != o4)
				return true;

			if (checkForColinear)
			{
				// Special Cases
				// p1, q1 and p2 are colinear and p2 lies on segment p1q1
				if (o1 == 0 && OnSegment(p1, p2, q1)) return true;

				// p1, q1 and p2 are colinear and q2 lies on segment p1q1
				if (o2 == 0 && OnSegment(p1, q2, q1)) return true;

				// p2, q2 and p1 are colinear and p1 lies on segment p2q2
				if (o3 == 0 && OnSegment(p2, p1, q2)) return true;

				// p2, q2 and q1 are colinear and q1 lies on segment p2q2
				if (o4 == 0 && OnSegment(p2, q1, q2)) return true;
			}

			return false; // Doesn't fall in any of the above cases
		}

		static public Vector2 IntersectionPoint(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2)
		{
			//does not handle case when segments are co linears.

			//general case
			if (p1.x != q1.x && p2.x != q2.x)
			{
				float yPerXFor1 = (q1.y - p1.y) / (q1.x - p1.x);
				float yPerXFor2 = (q2.y - p2.y) / (q2.x - p2.x);
				float yAtX0For1 = q1.y - yPerXFor1 * q1.x;
				float yAtX0For2 = q2.y - yPerXFor2 * q2.x;
				float xResolve = (yAtX0For2 - yAtX0For1) / (yPerXFor1 - yPerXFor2);

				return new Vector2(xResolve, xResolve * yPerXFor1 + yAtX0For1);
			}
			//first line is vertical
			else if (p1.x == q1.x)
			{
				float yPerXFor2 = (q2.y - p2.y) / (q2.x - p2.x);
				float yAtX0For2 = q2.y - yPerXFor2 * q2.x;

				return new Vector2(p1.x, p1.x * yPerXFor2 + yAtX0For2);
			}
			//second line is vertical
			else
			{
				float yPerXFor1 = (q1.y - p1.y) / (q1.x - p1.x);
				float yAtX0For1 = q1.y - yPerXFor1 * q1.x;

				return new Vector2(p2.x, p2.x * yPerXFor1 + yAtX0For1);
			}
		}

		public Vector3 ToV3(Vector2 v2, bool applyCreatorHeight = true)
		{
			return new Vector3(v2.x, applyCreatorHeight ? height : 0.0f, v2.y);
		}

		public void DrawEnvironmentHierarchy()
		{
			foreach (var environment in GetComponentsInChildren<Asset.World.Environment>())
			{
				if (environment == null)
				{
					continue;
				}
				else if (environment == this)
				{
					Vector2 parentPosition = (environment.transform.parent == null)
						? new Vector2()
						: new Vector2(environment.transform.parent.transform.position.x, environment.transform.parent.transform.position.z);
					for (int i = 0; i < environment.NumEdges; ++i)
					{
						Vector2 p1 = parentPosition + environment[i].Position;
						Vector2 p2 = parentPosition + environment[i + 1].Position;
						float scale = (Camera.current.transform.position - ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
						Handles.color = Color.black;
						Handles.DrawLine(ToV3(p1), ToV3(p2));
						Handles.ArrowHandleCap(
							0,
							ToV3((p1 + p2) / 2.0f),
							Quaternion.LookRotation(ToV3(new Vector2((p2 - p1).y, -(p2 - p1).x), false)),
							scale,
							EventType.Repaint);
					}
				}
				else
				{
					environment.DrawEnvironmentHierarchy();
				}
			}
		}
	}
}