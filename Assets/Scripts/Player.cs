using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Ability Q;
	public Ability W;
	public Ability E;
	public Ability R;
	
	public float speed;

	public Keybindings keybindings;
	public float resourceAffinity = 0.5f;

	private bool hasDestination;
	private Vector3 destination;
	private Vector3 direction;
	private float sqrMaxSpeed;

	// Use this for initialization
	void Start () {
		hasDestination = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (hasDestination)
		{
			Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
			if (Vector3.Dot(destination - transform.position, destination - newPosition) <= 0)
			{
				transform.position = destination;
				hasDestination = false;
			}
			else
				transform.position = newPosition;
		}
	}

	public void Move ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << LayerMask.NameToLayer("Terrain");
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, layerMask))
		{
			destination = hit.point;
			direction = destination - transform.position;
			direction.Normalize();
			hasDestination = true;
		}
	}
}
