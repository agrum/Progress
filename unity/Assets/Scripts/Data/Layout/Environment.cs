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
			foreach (var environment in node["nestedEnvironments"].AsArray)
			{
				NestedEnvironments.Add(new Environment(environment));
			}
		}

		public Environment(Environment other_) : base(other_)
		{
			Variety = other_.Variety;
			HeightDelta = other_.HeightDelta;
			foreach (var environment in other_.NestedEnvironments)
			{
				NestedEnvironments.Add(new Environment(environment));
			}
		}

		override public Edge this[int i]
		{
			get
			{
				i = (i + NumEdges) % NumEdges;
				var edge = EdgeList[i];
				edge.PreviousEdge = EdgeList[(i - 1 + NumEdges) % NumEdges];
				return edge;
			}
		}

		public bool Contains(Vector2 coordinate)
		{
			Vector2 p1 = coordinate;
			Vector2 q1 = new Vector2(coordinate.x, 1000000);

			int hitCount = 0;
			for (var i = 0; i < NumEdges; ++i)
			{
				Vector2 p2 = this[i].PreviousEdge.Position;
				Vector2 q2 = this[i].Position;

				if (Utility.Math.DoIntersect(p1, q1, p2, q2, true))
				{
					++hitCount;
				}
			}

			return (hitCount % 2) > 0;
		}

		public int HeightDeltaAsInt()
        {
			switch (HeightDelta)
            {
				case EHeightDelta.None: return 0;
				case EHeightDelta.Increase: return 1;
				case EHeightDelta.Decrease: return -1;
            }
			return 0;
        }
	}
}