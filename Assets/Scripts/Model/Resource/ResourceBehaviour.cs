using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class ResourceBehaviour : Expirable
	{
		public float AbsoluteRecharge { get; internal set; }
		public float RelativeRecharge { get; internal set; }
		public float ResplenishMultiplier { get; internal set; }
		public float DepleteMultiplier { get; internal set; }

		public ResourceBehaviour(float absoluteRecharge, float relativeRecharge, float resplenishMultiplier, float depleteMultiplier, float expirationTime = 0.0f) :
			base(expirationTime)
		{
			AbsoluteRecharge = absoluteRecharge;
			RelativeRecharge = relativeRecharge;
			ResplenishMultiplier = resplenishMultiplier;
			DepleteMultiplier = depleteMultiplier;
		}
	}
}