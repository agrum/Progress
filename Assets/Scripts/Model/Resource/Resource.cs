using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Resource {
	public enum ResourceType
	{
		Life,
		Armor,
		Shield,
		BurstShield
	}

	protected ResourceType Type;
	protected List<ResourceBehaviour> ResourceBehaviourList = new List<ResourceBehaviour>();
	protected List<ResourceReserve> ResourceReserveList = new List<ResourceReserve>();
	public float Amount { get; internal set; }
	public float Max { get; internal set; }
	public float ResplenishMultiplier { get; internal set; }
	public float DepleteMultiplier { get; internal set; }
	public float RechargePerSecond { get; internal set; }

	public delegate void Emptied(Resource resource);
	public event Emptied EmptiedEvent;
	public delegate void Filled(Resource resource);
	public event Filled FilledEvent;

	public void AddBehaviour(ResourceBehaviour behaviour)
	{
		ResourceBehaviourList.Add(behaviour);
		behaviour.ExpiredEvent += ResourceBehaviourExpired;
		UpdateRechargeRate();
	}

	virtual public void AddReserve(ResourceReserve reserve)
	{
		ResourceReserveList.Add(reserve);
		Amount += reserve.Amount;
		Max += reserve.Max;
		reserve.EmptiedEvent += ResourceReserveEmptied;
		reserve.ExpiredEvent += ResourceReserveExpired;
		UpdateRechargeRate();
		if (Amount >= Max)
			FilledEvent(this);
	}

	private void ResourceBehaviourExpired(Expirable behaviour)
	{
		ResourceBehaviourList.Remove(behaviour as ResourceBehaviour);
		UpdateRechargeRate();
	}

	private void ResourceReserveEmptied(ResourceReserve reserve)
	{
		if (reserve.DeleteWhenEmpty)
			ResourceReserveExpired(reserve);
	}

	private void ResourceReserveExpired(Expirable expirable)
	{
		var reserve = expirable as ResourceReserve;
		Amount -= reserve.Amount;
		Max -= reserve.Max;
		ResourceReserveList.Remove(reserve);
		UpdateRechargeRate();
		if(ResourceReserveList.Count == 0)
			EmptiedEvent(this);
	}

	virtual public void Deplete(ref Hit hit)
	{
		if (hit.Amount <= 0.0f)
			return;

		if (hit.Target == Type)
			hit.Amount *= DepleteMultiplier;

		float residualAmount = hit.Amount;
		for (int i = ResourceReserveList.Count - 1; i >= 0 && residualAmount == 0; --i)
			residualAmount = ResourceReserveList[i].Deplete(residualAmount);

		if(residualAmount == 0.0f)
			Amount -= hit.Amount;
		else
		{
			Amount = 0.0f;
			EmptiedEvent(this);
		}

		hit.Amount = residualAmount;
	}

	virtual public void Resplenish(ref Hit hit)
	{
		if (hit.Amount <= 0.0f)
			return;

		if(hit.Target == Type)
			hit.Amount *= ResplenishMultiplier;

		float residualAmount = hit.Amount;
		for (int i = 0; i < ResourceReserveList.Count && residualAmount == 0; ++i)
			residualAmount = ResourceReserveList[i].Resplenish(residualAmount);

		if (residualAmount == 0.0f)
			Amount += hit.Amount;
		else
		{
			Amount += hit.Amount - residualAmount;
			FilledEvent(this);
		}

		hit.Amount = residualAmount;
	}

	virtual public void Update(float dt)
	{
		Hit hit = new Hit(RechargePerSecond * dt / 1000.0f, Hit.HitType.Generic, Type);
		if (hit.Amount > 0)
			Resplenish(ref hit);
		else
			Deplete(ref hit);
	}

	private void UpdateRechargeRate()
	{
		float AbsoluteRecharge = 0.0f;
		float RelativeRecharge = 0.0f;
		ResplenishMultiplier = 1.0f;
		DepleteMultiplier = 1.0f;

		foreach (var behaviour in ResourceBehaviourList)
		{
			AbsoluteRecharge += behaviour.AbsoluteRecharge;
			RelativeRecharge += behaviour.RelativeRecharge;
			ResplenishMultiplier += behaviour.ResplenishMultiplier;
			DepleteMultiplier += behaviour.DepleteMultiplier;
		}
		
		RechargePerSecond = AbsoluteRecharge + RelativeRecharge * Max;
	}
}
