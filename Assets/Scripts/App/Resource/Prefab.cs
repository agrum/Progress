using UnityEngine;

namespace West
{
	namespace Model
	{
		namespace Resource
		{
			public class Prefab
			{
				public GameObject ConstellationNode { get; private set; } = null;

				public Prefab()
				{
					ConstellationNode = UnityEngine.Resources.Load("Prefabs/ConstellationNode") as GameObject;
				}
			}
		}
	}
}
