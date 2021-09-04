using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West.Asset
{
	[System.Serializable]
	public class EnvironmentEdge
	{
		[SerializeField]
		private Vector2 position;

		private Vector2 normal = Vector2.zero;
		private Vector2 center = Vector2.zero;
		private EnvironmentEdge nextEdge = null;

		//private bool normalOutdated = true;
		//private bool centerOutdated = true;

		public Vector2 Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
				//normalOutdated = true;
				//centerOutdated = true;
			}
		}

		public Vector3 Position3(float y)
		{
			return new Vector3(position.x, y, position.y);
		}

		public EnvironmentEdge NextEdge
		{
			get
			{
				return nextEdge;
			}
			set
			{
				nextEdge = value;
				//normalOutdated = true;
				//centerOutdated = true;
			}
		}

		public Vector2 Center
		{
			get
			{
				if (/*centerOutdated && */nextEdge != null)
				{
					center = (position + nextEdge.position) / 2.0f;
					//centerOutdated = false;
				}
				return center;
			}
		}

		public Vector2 Normal
		{
			get
			{
				if (/*normalOutdated && */nextEdge != null)
				{
					normal = new Vector2(nextEdge.Position.y - position.y, -(nextEdge.Position.x - position.x));
					//normalOutdated = false;
				}
				return normal;
			}
		}

		public EnvironmentEdge(Vector2 position_, EnvironmentEdge nextEdge_ = null)
		{
			position = position_;
			nextEdge = nextEdge_;
		}
	}
}