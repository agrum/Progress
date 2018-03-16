using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class SpeedModifier : Expirable
	{
		public float factor { get; private set; }

		public SpeedModifier(float p_factor, float p_expirationTime = 0.0f) :
			base(p_expirationTime)
		{
			factor = p_factor;
		}
	}
}