using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace West
{
	public class BurstShield : Resource
	{
		public float DechargeRate = 75.0f;

		ResourceBehaviour InternalBehaviour;

		public BurstShield()
		{
			Type = Resource.ResourceType.Shield;
			InternalBehaviour = new ResourceBehaviour(DechargeRate, 0, 1, 1);
			AddBehaviour(InternalBehaviour);
		}

		public override void AddReserve(ResourceReserve reserve)
		{
			reserve.Max = 0.0f;

			base.AddReserve(reserve);
		}
	}
}