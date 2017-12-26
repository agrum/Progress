using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject player;
	public GameObject offsetObject;
	public float speed = 1.0f;

	private bool followPlayer = false;
	private int cursorEnterCount = 0;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - offsetObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (followPlayer)
			transform.position = player.transform.position + offset;
		else
		{
			if (cursorEnterCount > 0)
			{
				var x = (Input.mousePosition.x - (Screen.width / 2)) / Screen.width;
				var y = (Input.mousePosition.y - (Screen.height / 2)) / Screen.height;
				transform.position += new Vector3(y + x, 0, y - x) * speed;
			}
		}
	}

	public void CursorEntered()
	{
		++cursorEnterCount;
	}

	public void CursorLeft()
	{
		--cursorEnterCount;
	}
}
