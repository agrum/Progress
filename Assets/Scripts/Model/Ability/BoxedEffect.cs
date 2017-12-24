using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BoxedEffect : MonoBehaviour
{
	public bool IsSingleShot { get; internal set; }
	public float ExpirationTime { get; internal set; }
	private bool ranOnce = false;

	public delegate void ObjectEnterDelegate(GameObject gameObject);
	public delegate void ObjectLeaveDelegate(GameObject gameObject);

	public event ObjectEnterDelegate ObjectEnterEvent;
	public event ObjectLeaveDelegate ObjectLeaveEvent;

	BoxedEffect()
	{
		IsSingleShot = true;
	}

	BoxedEffect(float expirationTime)
	{
		IsSingleShot = false;
		ExpirationTime = expirationTime;
	}

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
			enabled = false;
		if(!IsSingleShot && Time.time > ExpirationTime)
			enabled = false;

		ranOnce = true;
	}
}
