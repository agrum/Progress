using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.Layout
{
	public class Edge
	{
		private Vector2 position;
		private Edge previousEdge = null;

		public Edge(JSONNode node)
		{
			position.x = node.AsArray[0];
			position.y = node.AsArray[1];
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public Edge PreviousEdge
		{
			get
			{
				return previousEdge;
			}
			set
			{
				previousEdge = value;
			}
		}

		public Vector2 Center
		{
			get
			{
				if (previousEdge != null)
				{
					return (position + previousEdge.position) / 2.0f;
				}
				return new Vector2();
			}
		}

		public Vector2 Normal
		{
			get
			{
				if (previousEdge != null)
				{
					return new Vector2(position.y - previousEdge.Position.y, -(position.x - previousEdge.Position.x));
				}
				return Vector2.up;
			}
		}

		public Edge(Vector2 position_, Edge previousEdge_ = null)
		{
			position = position_;
			previousEdge = previousEdge_;
		}
	}
}