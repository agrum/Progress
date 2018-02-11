using System.Collections;
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