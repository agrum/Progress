using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BoxedEffect : MonoBehaviour
{
	public bool IsSingleShot = false;
	private bool ranOnce = false;

	public delegate void ObjectEnterDelegate(GameObject gameObject);
	public delegate void ObjectLeaveDelegate(GameObject gameObject);

	public event ObjectEnterDelegate ObjectEnterEvent;
	public event ObjectLeaveDelegate ObjectLeaveEvent;

	void OnTriggerEnter(Collider other)
	{
		if(enabled)
			ObjectEnterEvent(other.gameObject);
	}

	void OnTriggerLeave(Collider other)
	{
		if (enabled)
			ObjectLeaveEvent(other.gameObject);
	}

	void FixedUpdate()
	{
		if (IsSingleShot && ranOnce)
		{
			ranOnce = true;
			enabled = false;
		}
	}
}
