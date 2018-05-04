using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Expirable
{
	private float ExpirationTime;

	public delegate void Expired(Expirable expriable);
	public event Expired ExpiredEvent;

	public Expirable(float expirationTime)
	{
		ExpirationTime = expirationTime;
	}

	public void Expire()
	{
		ExpiredEvent(this);
	}

	public async Task ScheduleExpiry()
	{
		if (ExpirationTime == 0.0f)
			return;

		Expirable expriable = this;
		await Task.Delay((int)((ExpirationTime - Time.time) * 1000.0f));
		expriable.Expire();
		expriable = null;
	}
}
