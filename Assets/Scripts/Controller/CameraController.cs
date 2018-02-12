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

			if (Input.mousePosition.x < scrollMargin && Input.mousePosition.x >= 0)
				amplitude = scrollMargin - Mathf.Max(0, Input.mousePosition.x);
			else if (Input.mousePosition.x > Screen.width - scrollMargin && Input.mousePosition.x < Screen.width)
				amplitude = scrollMargin - Screen.width + Mathf.Min(Screen.width, Input.mousePosition.x);

			if (Input.mousePosition.y < scrollMargin && Input.mousePosition.y >= 0)
				amplitude = Mathf.Max(amplitude, scrollMargin - Mathf.Max(0, Input.mousePosition.y));
			else if (Input.mousePosition.y > Screen.height - scrollMargin && Input.mousePosition.y < Screen.height)
				amplitude = Mathf.Max(amplitude, scrollMargin - Screen.height + Mathf.Min(Screen.height, Input.mousePosition.y));

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
