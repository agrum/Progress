using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace West
{
	public class Armor : Resource
	{
		public float ArmorPhysicalReduction = 5.0f;

		public Armor()
		{
			Type = Resource.ResourceType.Armor;
		}

		public override void Deplete(ref Hit hit)
		{
			if (hit.Type == Hit.HitType.Physical)
				hit.Amount -= ArmorPhysicalReduction;

			base.Deplete(ref hit);
		}
	}
}
