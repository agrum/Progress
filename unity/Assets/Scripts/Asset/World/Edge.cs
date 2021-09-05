using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West.Asset.World
{
	[System.Serializable]
	public class Edge
	{
		[SerializeField]
		private Vector2 position;

		private Vector2 normal = Vector2.zero;
		private Vector2 center = Vector2.zero;

		private Edge previousEdge = null;

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

		public Vector3 Position3(float y)
		{
			return new Vector3(position.x, y, position.y);
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
					center = (position + previousEdge.position) / 2.0f;
				}
				return center;
			}
		}

		public Vector2 Normal
		{
			get
			{
				if (previousEdge != null)
				{
					normal = new Vector2(previousEdge.Position.y - position.y, -(previousEdge.Position.x - position.x));
				}
				return normal;
			}
		}

		public Edge(Vector2 position_, Edge previousEdge_ = null)
		{
			position = position_;
			previousEdge = previousEdge_;
		}
	}
}