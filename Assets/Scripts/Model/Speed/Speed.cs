using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class Speed
	{
		public float amount { get; private set; }
		private List<SpeedModifier> modifierList = new List<SpeedModifier>();

		public Speed()
		{
			UpdateSpeed();
		}

		public void AddModifier(SpeedModifier modifier)
		{
			if (modifier.factor <= 0.0f)
				return;

			modifierList.Add(modifier);
			modifier.ExpiredEvent += SpeedModifierExpired;
			UpdateSpeed();
		}

		void SpeedModifierExpired(Expirable behaviour)
		{
			modifierList.Remove(behaviour as SpeedModifier);
			UpdateSpeed();
		}

		void UpdateSpeed()
		{
			float localAmount = 1.0f;
			foreach (var modifier in modifierList)
			{
				localAmount *= modifier.factor;
			}
			amount = localAmount;
		}
	}
}