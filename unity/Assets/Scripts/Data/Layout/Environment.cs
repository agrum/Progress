using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Data.Layout
{
	public class Environment : LinearFeature
	{
		public enum EVariety
		{
			Grass,
			Dirt,
			Rock,
			Snow,
			Ocean,
		}

		public enum EHeightDelta
		{
			None,
			Increase,
			Decrease,
		}

		public EVariety Variety;
		public EHeightDelta HeightDelta;
		public List<Environment> NestedEnvironments = new List<Environment>();

		public Environment(JSONNode node) : base(node)
		{
			Variety = Data.Serializer.ReadEnum<EVariety>(node["variety"]);
			HeightDelta = Data.Serializer.ReadEnum<EHeightDelta>(node["heightDelta"]);
			foreach (var environment in node["environments"].AsArray)
			{
				NestedEnvironments.Add(new Environment(environment));
			}
		}

		override public Edge this[int i]
		{
			get
			{
				i = (i + NumEdges) % NumEdges;
				var edge = edgeList[i];
				edge.PreviousEdge = edgeList[(i - 1 + NumEdges) % NumEdges];
				return edge;
			}
		}
	}
}