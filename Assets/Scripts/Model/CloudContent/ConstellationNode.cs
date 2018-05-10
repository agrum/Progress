using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using BestHTTP;
using UnityEngine;

namespace West
{
	namespace Model
	{
		namespace CloudContent
		{
			public class ConstellationNode : Base
			{
				public GameObject Prefab { get; private set; } = null;
				public Material AbilityMaterial { get; private set; } = null;
				public Material ClassMaterial { get; private set; } = null;
				public Material KitMaterial { get; private set; } = null;

				protected override void Build(OnBuilt onBuilt_)
				{
					Prefab = Resources.Load("Prefabs/ConstellationNode") as GameObject;
					AbilityMaterial = Resources.Load("Colors/Blue") as Material;
					ClassMaterial = Resources.Load("Colors/Green") as Material;
					KitMaterial = Resources.Load("Colors/Red") as Material;

					onBuilt_();
				}

				public ConstellationNode(Session session_)
				{
					dependencyList.Add(session_);
				}
			}
		}
	}
}
