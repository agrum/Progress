using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceReserve : Expirable
{
	public Resource.ResourceType Type { get; internal set; }
	public float Amount { get; internal set; }
	public float Max { get; internal set; }
	//private float MaxProperty;
	public bool DeleteWhenEmpty { get; internal set; }
	public bool Full { get; internal set; }
	public bool Empty { get; internal set; }

	public delegate void Emptied(ResourceReserve reserve);
	public event Emptied EmptiedEvent;
	public delegate void Filled(ResourceReserve reserve);
	public event Filled FilledEvent;

	public ResourceReserve(Resource.ResourceType type, float amount, float max, bool deleteWhenEmpty, float expirationTime = 0.0f) :
		base(expirationTime)
	{
		Type = type;
		Amount = amount;
		Max = max;
		DeleteWhenEmpty = deleteWhenEmpty;
	}

	public float Deplete(float amount)
	{
		Amount -= amount;

		if (Amount < Max)
			Full = false;

		if (Amount <= 0.0f)
		{
			float excess = -Amount;
			Amount = 0.0f;
			Empty = true;
			EmptiedEvent(this);
			return excess;
		}

		return 0.0f;
	}

	public float Resplenish(float amount)
	{
		Amount += amount;

		if (Amount > 0.0f)
			Empty = false;

		if (Amount >= Max)
		{
			float excess = Amount - Max;
			Amount = Max;
			Full = true;
			FilledEvent(this);
			return excess;
		}

		return 0;
	}
}
