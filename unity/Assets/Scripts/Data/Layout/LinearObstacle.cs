using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Data.Layout
{
	public class LinearObstacle : LinearFeature
	{
		public enum EVariety
		{
			River,
			Wall,
			Fissure,
		}

		public EVariety Variety;

		public LinearObstacle(JSONNode node) : base(node)
		{
			Variety = Data.Serializer.ReadEnum<EVariety>(node["variety"]);
		}

		public LinearObstacle(LinearObstacle other_) : base(other_)
		{
			Variety = other_.Variety;
		}
	}
}