using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class Shield : Resource
	{
		public float RechargeRate = 25.0f;
		public float ResetPeriod = 2.0f;

		float LastHitReceived = 0.0f;
		bool RechargeDisabled = false;
		ResourceBehaviour InternalBehaviour;

		public Shield()
		{
			Type = Resource.ResourceType.Shield;
			InternalBehaviour = new ResourceBehaviour(RechargeRate, 0, 1, 1);
			AddBehaviour(InternalBehaviour);
		}

		override public void Deplete(ref Hit hit)
		{
			if (hit.Amount > 0.0f)
			{
				LastHitReceived = Time.time;
				if (!RechargeDisabled)
				{
					InternalBehaviour.Expire();
					RechargeDisabled = true;
				}
			}

			base.Deplete(ref hit);
		}

		public override void Update(float dt)
		{
			if (RechargeDisabled && Time.time - LastHitReceived >= ResetPeriod)
			{
				AddBehaviour(InternalBehaviour);
				RechargeDisabled = false;
			}

			base.Update(dt);
		}
	}
}