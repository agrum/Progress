using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	[System.Serializable]
	public class Edge
	{
		public enum TypeEnum
		{
			BlocksMovement,
			BlocksVisionAndMovement
		}

		[SerializeField]
		private Vector2 position;
		[SerializeField]
		private TypeEnum type;

		private Vector2 normal = Vector2.zero;
		private Vector2 center = Vector2.zero;
		private Edge nextEdge = null;

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

		public TypeEnum Type
		{
			get { return type; }
		}

		public Edge NextEdge
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

		public void RotateType()
		{
			switch (type)
			{
				case Edge.TypeEnum.BlocksMovement: type = Edge.TypeEnum.BlocksVisionAndMovement; break;
				case Edge.TypeEnum.BlocksVisionAndMovement: type = Edge.TypeEnum.BlocksMovement; break;
				default: type = Edge.TypeEnum.BlocksVisionAndMovement; break;
			}
		}

		public Edge(Vector2 _position, Edge _nextEdge = null, TypeEnum _type = TypeEnum.BlocksVisionAndMovement)
		{
			position = _position;
			nextEdge = _nextEdge;
			type = _type;
		}
	}
}