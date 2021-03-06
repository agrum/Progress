﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class RaycastPathHit
	{
		public Path path;
		public int index;
		public Vector2 origin;
		public Vector2 collision;
		//public float dst;
		//public float angle;
		//public float normal;

		public RaycastPathHit(Path _path, int _index, Vector2 _origin, Vector2 _collision)
		{
			path = _path;
			index = _index;
			origin = _origin;
			collision = _collision;
			//dst = _dst;
			//angle = _angle;
			//normal = _normal;
		}
	}

	[System.Serializable]
	public class Path : MonoBehaviour
	{

		[SerializeField, HideInInspector]
		public List<Edge> edgeList;

		[SerializeField, HideInInspector]
		public Vector2 center;

		[SerializeField, HideInInspector]
		public float height;

		[SerializeField, HideInInspector]
		public bool justDropped;

		private float roughRadius;
		private CircleCollider2D circleCollider;
		private PolygonCollider2D polyCollider;

		public void InitPath()
		{
			center = new Vector2(transform.position.x, transform.position.z);
			height = 0;
			edgeList = new List<Edge>();
			edgeList.Add(new Edge(center + Vector2.left, null, Edge.TypeEnum.BlocksVisionAndMovement));
			edgeList.Add(new Edge(center + Vector2.right, this[0], Edge.TypeEnum.BlocksVisionAndMovement));
			justDropped = true;
		}

		public void Start()
		{
			GameObject colliderContainer = TerrainManager.Instance.ColliderContainer;

			GameObject pathCollidersContainer = new GameObject(name + "_Colliders");
			pathCollidersContainer.transform.parent = colliderContainer.transform;

			GameObject circleColliderGO = new GameObject("CircleCollider");
			circleCollider = circleColliderGO.AddComponent<CircleCollider2D>();
			circleColliderGO.AddComponent<ColliderToPath>().path = this;
			circleColliderGO.transform.parent = pathCollidersContainer.transform;
			circleColliderGO.layer = TerrainManager.Instance.TerrainRoughColliderLayer;

			GameObject polyColliderGO = new GameObject("PolyCollider");
			polyCollider = polyColliderGO.AddComponent<PolygonCollider2D>();
			polyColliderGO.AddComponent<ColliderToPath>().path = this;
			polyColliderGO.transform.parent = pathCollidersContainer.transform;
			polyColliderGO.layer = gameObject.layer;

			UpdateUnityColliders();
		}

		public Edge this[int i]
		{
			get
			{
				Edge edge = edgeList[i % NumEdges];
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
			edgeList.Insert((i % NumEdges) + 1, new Edge((this[i + 1].Position + this[i].Position) / 2.0f, this[i + 1], this[i].Type));
		}

		public int NumEdges
		{
			get { return edgeList.Count; }
		}

		public float RoughRadius
		{
			get { return roughRadius; }
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

		public void UpdateUnityColliders()
		{
			//circle collider
			circleCollider.transform.position = new Vector3(center.x, center.y, 0);
			float radius = 0.0f;
			for (int i = 0; i < NumEdges; ++i)
			{
				radius = Mathf.Max(radius, (this[i].Position - center).magnitude);
			}
			roughRadius = radius;
			circleCollider.radius = radius;

			//polygon collider
			polyCollider.transform.position = new Vector3(center.x, center.y, 0);
			List<Vector2> points = new List<Vector2>();
			foreach (var edge in edgeList)
				points.Add(edge.Position - center);
			polyCollider.SetPath(0, points.ToArray());
		}

		public bool Raycast(Vector3 origin, Vector3 direction, ref RaycastPathHit hit, float length, Edge.TypeEnum filter)
		{
			Vector2 direction2D = new Vector2(direction.x, direction.z).normalized;
			Vector2 p1 = new Vector2(origin.x, origin.z);
			Vector2 p2 = p1 + direction2D * length;

			bool collided = false;

			for (int i = 0; i < NumEdges; ++i)
			{
				if (this[i].Type == filter && IsFacing(direction2D, this[i].Position, this[i + 1].Position))
				{
					if (DoIntersect(p1, p2, this[i].Position, this[i + 1].Position, false))
					{
						Vector2 intersectionPoint = IntersectionPoint(p1, p2, this[i].Position, this[i + 1].Position);
						if (hit.path == null || (intersectionPoint - p1).sqrMagnitude < (hit.collision - hit.origin).sqrMagnitude)
						{
							hit.path = this;
							hit.index = i;
							hit.origin = p1;
							hit.collision = intersectionPoint;
						}
						collided = true;
					}
				}
			}

			return collided;
		}

		bool IsFacing(Vector2 direction, Vector2 p, Vector2 q)
		{
			Vector2 directionRotated = new Vector2(direction.y, -direction.x); //rotate 90*
			return Vector2.Dot(directionRotated, q - p) > 0.0f;
		}

		// Given three colinear edgeList p, q, r, the function checks if
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
				float yAtX0For2 = q1.y - yPerXFor2 * q1.x;

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
	}
}