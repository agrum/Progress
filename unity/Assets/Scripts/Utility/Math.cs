using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class Math
    {
		// Given three points p, q, r, the function checks if
		// point q lies on line segment 'pr'
		static public bool OnSegment(Vector2 p, Vector2 q, Vector2 r)
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
		static public int Orientation(Vector2 p, Vector2 q, Vector2 r)
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
		static public bool DoIntersect(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2, bool checkForColinear)
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
			if (p1 == p2 || p1 == q2)
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
	}
}