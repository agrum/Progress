using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace West.Asset.World
{
	[System.Serializable]
	public class LinearFeature : MonoBehaviour
	{
		[SerializeField]
		public string name;

		[SerializeField, HideInInspector]
		public List<Edge> edgeList;

		[SerializeField, HideInInspector]
		public Vector2 center;

		[SerializeField, HideInInspector]
		public float height;

		[SerializeField, HideInInspector]
		public bool justDropped;


		virtual public void Init()
		{
			center = new Vector2(transform.localPosition.x, transform.localPosition.z);
			height = transform.position.y;
			edgeList = new List<Edge>();
			edgeList.Add(new Edge(center + Vector2.down + Vector2.right));
			edgeList.Add(new Edge(center + Vector2.up + Vector2.left));
			justDropped = true;
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

		public Bounds Bounds
        {
			get
			{
				Vector2 min = new Vector2(Mathf.Infinity, Mathf.Infinity);
				Vector2 max = new Vector2(-Mathf.Infinity, -Mathf.Infinity);
				Vector2 parentPosition = ParentPosition;
				foreach (var edge in edgeList)
				{
					min.x = Mathf.Min(edge.Position.x, min.x);
					min.y = Mathf.Min(edge.Position.y, min.y);
					max.x = Mathf.Max(edge.Position.x, max.x);
					max.y = Mathf.Max(edge.Position.y, max.y);
				}

				min.x += parentPosition.x;
				min.y += parentPosition.y;
				max.x += parentPosition.x;
				max.y += parentPosition.y;
				Vector3 center = new Vector3((min.x + max.x) / 2, 0, (min.y + max.y) / 2);
				Vector3 size = new Vector3(max.x - min.x, 0, max.y - min.y);
				var b = new Bounds(center, size);
				return b;
			}
		}

		virtual public Color Color
        {
			get
            {
				return Color.black;
            }
        }

		public Vector2 ParentPosition => new Vector2(transform.position.x - transform.localPosition.x, transform.position.z - transform.localPosition.z);

		public void Merge(int i)
		{
			i = (i + NumEdges) % NumEdges;
			var edge = this[i];
			if (NumEdges > 2 && edge.PreviousEdge != null)
			{
				edge.PreviousEdge.Position = (edge.PreviousEdge.Position + edge.Position) / 2.0f;
				edgeList.RemoveAt(i);
				edge = this[i];
			}
		}

		public void Split(int i)
		{
			var edge = this[i];
			if (NumEdges >= 2 && edge.PreviousEdge != null)
			{
				edgeList.Insert((i % NumEdges), new Edge((edge.PreviousEdge.Position + edge.Position) / 2.0f));
				edge = this[i+1];
				edge = this[i+2];
			}
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

		public bool CollidesWith(LinearFeature other)
		{
			Vector2 parentPosition = ParentPosition;
			Vector2 OtherParentPosition = other.ParentPosition;
			for (int i = 0; i < NumEdges; ++i)
			{
				if (this[i].PreviousEdge == null)
				{
					continue;
				}

				Vector2 p1 = parentPosition + this[i - 1].Position;
				Vector2 p2 = parentPosition + this[i].Position;
				for (int j = 0; j < other.NumEdges; ++j)
				{
					Vector2 p3 = OtherParentPosition + other[j - 1].Position;
					Vector2 p4 = OtherParentPosition + other[j].Position;
					if (DoIntersect(p1, p2, p3, p4, true))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsSelfIntersecting()
		{
			for (var i = 0; i < edgeList.Count; ++i)
			{
				for (var j = i + 1; j < edgeList.Count; ++j)
				{
					if (edgeList[i].PreviousEdge == null
						|| edgeList[j].PreviousEdge == null
						|| edgeList[i].PreviousEdge == edgeList[j]
						|| edgeList[j].PreviousEdge == edgeList[i])
					{
						continue;
					}

					if (DoIntersect(edgeList[i].PreviousEdge.Position, edgeList[i].Position, edgeList[j].PreviousEdge.Position, edgeList[j].Position, true))
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
		protected bool DoIntersect(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2, bool checkForColinear)
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
			//sharing point case
			if (p1 == p2 || p1 == q2 )
            {
				return p1;
            }
			if (q1 == p1 || q1 == q2)
            {
				return q1;
            }

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

		public void DrawHierarchy()
		{
			foreach (var linearFeature in GetComponentsInChildren<Asset.World.LinearFeature>())
			{
				if (linearFeature == null)
				{
					continue;
				}
				else if (linearFeature == this)
				{
					Vector2 parentPosition = ParentPosition;
					for (int i = 0; i < linearFeature.NumEdges; ++i)
					{
						if (linearFeature[i].PreviousEdge == null)
                        {
							continue;
                        }

						Vector2 p1 = parentPosition + linearFeature[i - 1].Position;
						Vector2 p2 = parentPosition + linearFeature[i].Position;
						float scale = (Camera.current.transform.position - ToV3((p1 + p2) / 2.0f)).magnitude / 50.0f;
						Handles.color = Color;
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
					linearFeature.DrawHierarchy();
				}
			}
		}
	}
}