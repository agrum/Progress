using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {
	public Player player;

	public void AddBehaviour(ResourceBehaviour behaviour)
	{
		Resource resource = null;
		foreach (var subResource in player.healthResourceList)
		{
			if (subResource.Type == behaviour.Type)
			{
				resource = subResource;
				break;
			}
		}
		if (resource == null)
		{
			resource = new Resource();
			resource.Type = behaviour.Type;
			int index = 0;
			foreach (var subResource in player.healthResourceList)
			{
				if (resource.Type.HitPriority > subResource.Type.HitPriority)
				{
					player.healthResourceList.Insert(index, resource);
					break;
				}
				++index;
			}
			if (index == player.healthResourceList.Count)
				player.healthResourceList.Add(resource);
		}
		resource.AddBehaviour(behaviour);
	}

	public float Hit(float amount)
	{
		//if negative impact hit, hit front resources first
		if (amount < 0)
		{
			for (
				var index = 0; 
				amount > 0 && index < player.healthResourceList.Count; 
				++index)
			{
				amount = Hit(amount, player.healthResourceList[index]);
			}
		}
		//else hit back resource first (life, then others)
		else
		{
			for (
				var index = player.healthResourceList.Count - 1; 
				amount > 0 && index >= 0; 
				--index)
			{
				amount = Hit(amount, player.healthResourceList[index]);
			}
		}

		return amount;
	}

	private float Hit(float amount, Resource resourceHit)
	{
		return resourceHit.Hit(amount);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
