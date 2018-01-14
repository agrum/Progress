using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public enum AbilityState
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
	public float speedBase;
	public Keybinding keybinding;
	public float resourceAffinity = 0.5f;

	internal Speed speed = new Speed();

	private AbilityState state = AbilityState.None;
	private bool hasDestination = false;
	private Vector3 destination;
	private Vector3 direction;
	private float sqrMaxSpeed;
	private bool canCast = true;
	private Ability activeAbility = null;

	internal AbilityState State
	{
		get
		{
			return this.state;
		}
		set
		{
			this.state = value;
			if(value == AbilityState.Casting)
				hasDestination = false;
		}
	}

	public void Move()
	{
		if(state != AbilityState.Casting)
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
	
	void Start ()
	{
		abilityQ.keybind = keybinding.q;
		abilityW.keybind = keybinding.w;
		abilityE.keybind = keybinding.e;
		abilityR.keybind = keybinding.q;
		abilityQ.player = this;
		abilityW.player = this;
		abilityE.player = this;
		abilityR.player = this;

		state = AbilityState.None;
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
			Vector3 newPosition = transform.position + direction * speedBase * speed.amount * Time.deltaTime;
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

		if (state == AbilityState.None)
			activeAbility = null;
		if (newActiveAbility && newActiveAbility.CanActivate() && state != AbilityState.Casting)
		{
			if (activeAbility && activeAbility != newActiveAbility && state != AbilityState.Aiming)
				activeAbility.Cancel();
			activeAbility = newActiveAbility;
			activeAbility.Activate();
		}
	}
}
