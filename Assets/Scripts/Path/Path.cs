using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path {

	[SerializeField, HideInInspector]
	List<Vector2> points = new List<Vector2>();

	public Path(Vector2 center)
	{
		points.Add(center + Vector2.left);
		points.Add(center + Vector2.right);
	}

	public Vector2 this[int i]
	{
		get
		{
			return points[i];
		}
		set
		{
			points[i] = value;
		}
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
		points.Add(anchorPoint);
	}

	public Vector2[] GetPointsInSegment(int i)
	{
		return new Vector2[] { points[i], points[i+1] };
	}
}
