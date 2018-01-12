using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPunch : Ability
{
	public float startRange = 3.0f;
	public float chargedRange = 10.0f;
	public float width = 2.0f;

	private GameObject aim;

	override public void Activate()
	{
		aim = Instantiate(Resources.Load("Prefabs/Decals/Abilities/Composed/Line", typeof(GameObject))) as GameObject;
		aim.transform.SetParent(player.transform, false);
		aim.transform.localScale = new Vector3(width, 1.0f, startRange);
	}
}
