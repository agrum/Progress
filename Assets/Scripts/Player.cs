using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	enum AbilityState
	{
		None,
		Aiming,
		Casting,
		Channeling
	}

	public Ability abilityQ;
	public Ability abilityW;
	public Ability abilityE;
	public Ability abilityR;
	
	public float speed;

	public Keybinding keybinding;
	public float resourceAffinity = 0.5f;

	private AbilityState actionState = AbilityState.None;
	private bool hasDestination = false;
	private Vector3 destination;
	private Vector3 direction;
	private float sqrMaxSpeed;
	private bool canCast = true;
	private Ability activeAbility = null;

	public void Move()
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
	
	void Start ()
	{
		abilityQ.keybind = keybinding.q;
		abilityW.keybind = keybinding.w;
		abilityE.keybind = keybinding.e;
		abilityR.keybind = keybinding.q;
	}
	
	void Update()
	{
		UpdatePosition();
		TakeKeyboardInput();
	}

	void UpdatePosition()
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

	void TakeKeyboardInput()
	{
		Ability newActiveAbility = null;
		if (Input.GetKeyDown(keybinding.q))
			newActiveAbility = abilityQ;
		else if (Input.GetKeyDown(keybinding.w))
			newActiveAbility = abilityW;
		else if (Input.GetKeyDown(keybinding.e))
			newActiveAbility = abilityE;
		else if (Input.GetKeyDown(keybinding.r))
			newActiveAbility = abilityR;

		if (actionState == AbilityState.None)
			activeAbility = null;
		if (newActiveAbility && newActiveAbility.CanActivate() && actionState != AbilityState.Casting)
		{
			if (activeAbility && activeAbility != newActiveAbility && actionState != AbilityState.Aiming)
				activeAbility.Cancel();
			activeAbility = newActiveAbility;
			activeAbility.Activate();
		}
	}
}
