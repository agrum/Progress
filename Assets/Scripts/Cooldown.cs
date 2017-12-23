using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour {
	public float BaseDuration = 0;

	private float ConsumedTimestamp = 0;
	private float CooledTimestamp = 0;

	public void Consume ()
	{
		ConsumedTimestamp = Time.time;
		CooledTimestamp = Time.time + BaseDuration;
	}

	public float TimeUntilCooled ()
	{
		return (CooledTimestamp > Time.time) ? CooledTimestamp - Time.time : 0.0f;
	}

	public float CooledPercentage ()
	{
		if (ConsumedTimestamp == CooledTimestamp)
			return 1.0f;

		return 1.0f - TimeUntilCooled() / (CooledTimestamp - ConsumedTimestamp);
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
