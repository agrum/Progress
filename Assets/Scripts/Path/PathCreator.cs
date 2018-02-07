using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour {

	public float height = 0.0f;

	[HideInInspector]
	public Path path;

	public void CreatePath()
	{
		path = new Path(transform.position);
	}
}
