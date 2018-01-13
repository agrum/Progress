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

	private GameObject aim;
	private bool isAiming = false;
	private float startTime = 0.0f;
	private SpeedModifier speedModifier;
	private float orientation = 0.0f;

	override public void Activate()
	{
		aim = Instantiate(Resources.Load("Prefabs/Decals/Abilities/Composed/Line", typeof(GameObject))) as GameObject;
		aim.transform.SetParent(player.transform, false);
		aim.transform.localScale = new Vector3(width, 1.0f, startRange);
		isAiming = true;
		startTime = Time.time;
		speedModifier = new SpeedModifier(speedReduction);
		player.speed.AddModifier(speedModifier);
	}

	void Update()
	{
		if(isAiming)
		{
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
				aim.transform.localEulerAngles = new Vector3(0.0f, orientation, 0.0f);
			}

			if (Input.GetKeyDown(keybind))
			{

			}
			else if (Input.GetKeyDown(keybind))
			{

			}
		}
	}
}
