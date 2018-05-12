using UnityEngine;

namespace West
{
	namespace Model
	{
		namespace Resource
		{
			public class Material
			{
				public UnityEngine.Material AbilityMaterial { get; private set; } = null;
				public UnityEngine.Material ClassMaterial { get; private set; } = null;
				public UnityEngine.Material KitMaterial { get; private set; } = null;

				public Material()
				{
					AbilityMaterial = UnityEngine.Resources.Load("Colors/Blue") as UnityEngine.Material;
					ClassMaterial = UnityEngine.Resources.Load("Colors/Green") as UnityEngine.Material;
					KitMaterial = UnityEngine.Resources.Load("Colors/Red") as UnityEngine.Material;
				}
			}
		}
	}
}
