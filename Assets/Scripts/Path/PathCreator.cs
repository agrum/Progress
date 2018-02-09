using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour {
	
	[HideInInspector]
	public Path path;

	public void Start()
	{
		path = new Path(transform.position);
	}

	public void CreatePath()
	{
		path = new Path(transform.position);
	}
}
