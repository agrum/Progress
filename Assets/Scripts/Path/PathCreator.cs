using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour {

	[HideInInspector]
	public Path path;

	[HideInInspector]
	public bool justDropped = true;

	public void CreatePath()
	{
		path = new Path(transform.position);
	}
}
