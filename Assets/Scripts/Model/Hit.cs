using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace West
{
	public class Hit
	{
		public enum HitType
		{
			Generic,
			Physical,
			Energy
		}

		public float Amount { get; set; }
		public HitType Type { get; internal set; }
		public Resource.ResourceType Target { get; internal set; }

		public Hit(float amount, HitType type, Resource.ResourceType target)
		{
			Amount = amount;
			Type = type;
			Target = target;
		}
	}
}