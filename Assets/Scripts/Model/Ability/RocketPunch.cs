using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPunch : Ability
{
	public float startRange = 3.0f;
	public float chargedRange = 10.0f;
	public float width = 2.0f;
	public float chargeTime = 2.0f;
	public float maxHoldTime = 4.0f;
	public float speedReduction = 0.4f;
	public float travelTime = 0.5f;

	private GameObject aim;
	private bool isAiming = false;
	private bool isCasting = false;
	private float aimStartTime = 0.0f;
	private float castStartTime = 0.0f;
	private SpeedModifier speedModifier;
	private float orientation = 0.0f;
	private float charge = 0.0f;
	private Vector3 jump;

	override public void Activate()
	{
		aim = Instantiate(Resources.Load("Prefabs/Decals/Abilities/Composed/Line", typeof(GameObject))) as GameObject;
		aim.transform.SetParent(player.transform, false);
		aim.transform.localScale = new Vector3(width, 1.0f, startRange);
		isAiming = true;
		aimStartTime = Time.time;
		speedModifier = new SpeedModifier(speedReduction);
		player.speed.AddModifier(speedModifier);
		player.State = Player.AbilityState.Aiming;
		player.crouch = 0.5f;
	}

	void Update()
	{
		if(isAiming)
		{
			charge = Mathf.Min(1.0f, (Time.time - aimStartTime) / chargeTime);
			float distance = Mathf.Lerp(startRange, chargedRange, charge);
			aim.transform.localScale = new Vector3(width, 1.0f, distance);

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			int layerMask = 1 << LayerMask.NameToLayer("Terrain");
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100, layerMask))
			{
				var direction = hit.point - player.transform.position;
				direction.y = 0.0f;
				orientation = Vector3.Angle(new Vector3(0.0f, 0.0f, 1.0f), direction);
				float isRight = Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), direction);
				if (isRight < 0.0f)
					orientation *= -1.0f;
				player.facingDirection = orientation;
			}

			//switch from aim to cast
			if (Time.time > aimStartTime + maxHoldTime || Input.GetKeyUp(keybind))
			{
				player.State = Player.AbilityState.Casting;
				isAiming = false;
				isCasting = true;
				player.crouch = 0.0f;
				speedModifier.Expire();
				speedModifier = null;
				jump = new Vector3(Mathf.Sin((orientation) * Mathf.Deg2Rad), 0.0f, Mathf.Cos((orientation) * Mathf.Deg2Rad)) * distance;

				Debug.Log(TerrainManager.Instance.CanJumpTo(player.transform.position + jump));

				Destroy(aim);
				castStartTime = Time.time;
			}
		}
		if(isCasting)
		{
			bool castFinished = false;
			float jumpDT = Time.deltaTime;
			if (Time.time > castStartTime + travelTime)
			{
				jumpDT -= Time.time - (castStartTime + travelTime);
				castFinished = true;
			}

			player.transform.position += jump * (jumpDT / travelTime);

			//switct state
			if(castFinished)
			{
				isCasting = false;
				player.State = Player.AbilityState.None;
			}
		}
	}
}
