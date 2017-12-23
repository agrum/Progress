using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Life : Resource
{
	Armor AttachedArmor;
	Shield AttachedShield;
	BurstShield AttachedBurstShield;

	public Life()
	{
		Type = Resource.ResourceType.Life;
	}

	override public void AddReserve(ResourceReserve reserve)
	{
		if (reserve.Type == Resource.ResourceType.Armor)
			AttachedArmor.AddReserve(reserve);
		else if (reserve.Type == Resource.ResourceType.Shield)
			AttachedShield.AddReserve(reserve);
		else if (reserve.Type == Resource.ResourceType.BurstShield)
			AttachedBurstShield.AddReserve(reserve);
		else
			base.AddReserve(reserve);
	}

	override public void Deplete(ref Hit hit)
	{
		if (hit.Target == Type)
			hit.Amount *= DepleteMultiplier;

		AttachedBurstShield.Deplete(ref hit);
		AttachedShield.Deplete(ref hit);
		AttachedArmor.Deplete(ref hit);

		if (hit.Target == Type)
			hit.Amount /= DepleteMultiplier; //it will be re-applied in the base version

		base.Deplete(ref hit);
	}

	override public void Resplenish(ref Hit hit)
	{
		base.Resplenish(ref hit);

		AttachedArmor.Resplenish(ref hit);
		AttachedShield.Resplenish(ref hit);
		AttachedBurstShield.Resplenish(ref hit);
	}
}
