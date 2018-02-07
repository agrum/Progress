﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Edge
{
	public enum TypeEnum
	{
		BlocksMovement,
		BlocksVision,
		BlocksBoth
	}

	public Vector2 position;
	public TypeEnum type;

	public Edge(Vector2 _position, TypeEnum _type)
	{
		position = _position;
		type = _type;
	}
}

[System.Serializable]
public class Path {

	[SerializeField, HideInInspector]
	List<Edge> points = new List<Edge>();

	public Path(Vector2 center)
	{
		points.Add(new Edge(center + Vector2.left, Edge.TypeEnum.BlocksMovement));

		points.Add(new Edge(center + Vector2.right, Edge.TypeEnum.BlocksMovement));
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
		points.RemoveAt(i % NumPoints());
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
		points.Add(new Edge(anchorPoint, Edge.TypeEnum.BlocksMovement));
	}
}
