using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject player;
	public GameObject offsetObject;
	public float speed = 1.0f;
	public float scrollMargin = 80.0f;

	private bool followPlayer = false;
	private int cursorEnterCount = 0;
	private Vector3 offset;
	private float invScrollMargin;
	private float invScrollMarginTimesSpeed;

	// Use this for initialization
	void Start () {
		offset = transform.position - offsetObject.transform.position;
		invScrollMargin = 1.0f / scrollMargin;
		invScrollMarginTimesSpeed = invScrollMargin * speed;
	}
	
	// Update is called once per frame
	void Update () {
		if (followPlayer)
			transform.position = player.transform.position + offset;
		else
		{
			var amplitude = 0.0f;

			if (Input.mousePosition.x < scrollMargin)
				amplitude = scrollMargin - Input.mousePosition.x;
			else if (Input.mousePosition.x > Screen.width - scrollMargin)
				amplitude = scrollMargin - Screen.width + Input.mousePosition.x;

			if (Input.mousePosition.y < scrollMargin)
				amplitude = Mathf.Max(amplitude, scrollMargin - Input.mousePosition.y);
			else if (Input.mousePosition.y > Screen.height - scrollMargin)
				amplitude = Mathf.Max(scrollMargin - Screen.height + Input.mousePosition.y);

			if (amplitude > 0.0f)
			{
				var x = (Input.mousePosition.x - Screen.width / 2.0f) / Screen.width / 2.0f;
				var y = (Input.mousePosition.y - Screen.height / 2.0f) / Screen.height / 2.0f;
				var v = new Vector3(x, 0, y);
				v.Normalize();
				transform.position += v * amplitude * invScrollMarginTimesSpeed;
			}
		}
	}
}
