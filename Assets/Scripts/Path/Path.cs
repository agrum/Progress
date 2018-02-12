using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPathHit
{
	public Path path;
	public Vector2 origin;
	public Vector2 collision;
	//public float dst;
	//public float angle;
	//public float normal;

	public RaycastPathHit(Path _path, Vector2 _origin, Vector2 _collision)
	{
		path = _path;
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

	[SerializeField]
	public List<Edge> points;

	[SerializeField]
	public Vector2 center;

	[SerializeField]
	public bool justDropped;

	public CircleCollider2D circleCollider;

	public void InitPath()
	{
		center = new Vector2(transform.position.x, transform.position.z);
		points = new List<Edge>();
		points.Add(new Edge(center + Vector2.left, Edge.TypeEnum.BlocksBoth));
		points.Add(new Edge(center + Vector2.right, Edge.TypeEnum.BlocksBoth));
		justDropped = true;
	}

	public Vector2 this[int i]
	{
		get
		{
			return points[i % NumPoints()].position;
		}
		set
		{
			points[i % NumPoints()].position = value;
		}
	}

	public Edge.TypeEnum GetType(int i)
	{
		return points[i % NumPoints()].type;
		
	}

	public void ChangeType(int i)
	{
		switch(points[i % NumPoints()].type)
		{
			case Edge.TypeEnum.BlocksMovement: points[i % NumPoints()].type = Edge.TypeEnum.BlocksVision; break;
			case Edge.TypeEnum.BlocksVision: points[i % NumPoints()].type = Edge.TypeEnum.BlocksBoth; break;
			case Edge.TypeEnum.BlocksBoth: points[i % NumPoints()].type = Edge.TypeEnum.BlocksMovement; break;
		}
	}

	public void Remove(int i)
	{
		if(NumPoints() > 2)
			points.RemoveAt(i % NumPoints());
	}

	public void Merge(int i)
	{
		if (NumPoints() > 2)
		{
			points[(i + 1) % NumPoints()].position = (points[(i + 1) % NumPoints()].position + points[i % NumPoints()].position) / 2.0f;
			points.RemoveAt(i % NumPoints());
		}
	}

	public void Split(int i)
	{
		points.Insert((i+1) % NumPoints(), new Edge((points[(i + 1) % NumPoints()].position + points[i % NumPoints()].position) / 2.0f, points[i].type));
	}

	public int NumPoints()
	{
		return points.Count;
	}

	public int NumSegments()
	{
		return points.Count - 1;
	}

	public void AddSegment(Vector2 anchorPoint)
	{
		points.Add(new Edge(anchorPoint, Edge.TypeEnum.BlocksBoth));
	}

	public bool Raycast(Vector3 origin, Vector3 direction, ref RaycastPathHit hit, float length, Edge.TypeEnum filter)
	{
		Vector2 direction2D = new Vector2(direction.x, direction.z).normalized;
		Vector2 p1 = new Vector2(origin.x, origin.z);
		Vector2 p2 = p1 + direction2D * length;

		bool collided = false;

		for (int i = 0; i < NumPoints(); ++i)
		{
			if (points[i].type == filter && IsFacing(direction2D, this[i], this[i + 1]))
			{
				if(DoIntersect(p1, p2, this[i], this[i + 1], false))
				{
					Vector2 intersectionPoint = IntersectionPoint(p1, p2, this[i], this[i + 1]);
					if (hit.path == null || (intersectionPoint - p1).sqrMagnitude < (hit.collision - hit.origin).sqrMagnitude)
					{
						hit.path = this;
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

	// Given three colinear points p, q, r, the function checks if
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
		// See https://www.geeksforgeeks.org/orientation-3-ordered-points/
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
		if(p1.x != q1.x && p2.x != q2.x)
		{
			float yPerXFor1 = (q1.y - p1.y) / (q1.x - p1.x);
			float yPerXFor2 = (q2.y - p2.y) / (q2.x - p2.x);
			float yAtX0For1 = q1.y - yPerXFor1 * q1.x;
			float yAtX0For2 = q2.y - yPerXFor2 * q2.x;
			float xResolve = (yAtX0For2 - yAtX0For1) / (yPerXFor1 - yPerXFor2);

			return new Vector2(xResolve, xResolve * yPerXFor1 + yAtX0For1);
		}
		//first line is vertical
		else if(p1.x == q1.x)
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
