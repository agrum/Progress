using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class TerrainManager
	{
		//static instantiation
		private static TerrainManager _instance;
		public static TerrainManager Instance
		{
			get
			{
				if (_instance == null)
					_instance = new TerrainManager();
				return _instance;
			}
		}
		//static instantiation

		int terrainRoughColliderLayer;
		int terrainColliderLayer;
		GameObject colliderContainer = null;
		GameObject dummyColliderGO = null;
		CircleCollider2D dummyCollider = null;
		Terrain terrain;

		private TerrainManager()
		{
			terrainRoughColliderLayer = LayerMask.NameToLayer("TerrainRoughCollider");
			terrainColliderLayer = LayerMask.NameToLayer("TerrainCollider");
			dummyColliderGO = new GameObject();
			dummyCollider = dummyColliderGO.AddComponent<CircleCollider2D>();
			terrain = GameObject.FindGameObjectsWithTag("Terrain")[0].GetComponent<Terrain>();
			var plop = TeamManager.Instance;
		}

		public int TerrainRoughColliderLayer
		{
			get { return terrainRoughColliderLayer; }
		}

		public int TerrainColliderLayer
		{
			get { return terrainColliderLayer; }
		}

		public GameObject ColliderContainer
		{
			get
			{
				if (colliderContainer == null)
				{
					GameObject[] unityColldier2DContainerArray = GameObject.FindGameObjectsWithTag("UnityColldier2DContainer");
					if (unityColldier2DContainerArray.Length > 0)
						colliderContainer = unityColldier2DContainerArray[0];
					else
					{
						GameObject unityColldier2DContainer = new GameObject("UnityColldier2DContainer");
						unityColldier2DContainer.tag = "UnityColldier2DContainer";
						colliderContainer = unityColldier2DContainer;
					}
				}

				return colliderContainer;
			}
		}

		public float SampleHeight(Vector2 position)
		{
			return terrain.SampleHeight(position);
		}

		public float SampleHeight(Vector3 position)
		{
			return terrain.SampleHeight(position);
		}

		public bool CanJumpTo(Vector3 position)
		{
			Vector2 position2D = new Vector2(position.x, position.z);
			foreach (CircleCollider2D circleCollider in ColliderContainer.GetComponentsInChildren(typeof(CircleCollider2D), true))
			{
				if (circleCollider.gameObject.layer == TerrainRoughColliderLayer)
				{
					if (circleCollider.OverlapPoint(position2D))
					{
						GameObject parent = circleCollider.transform.parent.gameObject;
						PolygonCollider2D[] polygonColliderArray = parent.GetComponentsInChildren<PolygonCollider2D>();
						if (polygonColliderArray.Length > 0 &&
							polygonColliderArray[0].gameObject.layer == terrainColliderLayer &&
							polygonColliderArray[0].OverlapPoint(position2D))
							return false;
					}
				}
			}

			return true;
		}

		public bool CanJumpTo(Vector3 position, float radius)
		{
			dummyColliderGO.transform.position = new Vector3(position.x, position.z, 0);
			dummyCollider.radius = radius;

			Vector2 position2D = new Vector2(position.x, position.z);
			foreach (CircleCollider2D circleCollider in ColliderContainer.GetComponentsInChildren(typeof(CircleCollider2D), true))
			{
				if (circleCollider.gameObject.layer == TerrainRoughColliderLayer)
				{
					if (circleCollider.IsTouching(dummyCollider))
					{
						GameObject parent = circleCollider.transform.parent.gameObject;
						PolygonCollider2D[] polygonColliderArray = parent.GetComponentsInChildren<PolygonCollider2D>();
						if (polygonColliderArray.Length > 0 &&
							polygonColliderArray[0].gameObject.layer == terrainColliderLayer &&
							polygonColliderArray[0].IsTouching(dummyCollider))
							return false;
					}
				}
			}

			return true;
		}

		public Vector3 ComputeAcceptableJump(Vector3 source, Vector3 jump, float jumperRadius)
		{
			Vector2 source2D = new Vector2(source.x, source.z);
			Vector2 jump2D = new Vector2(jump.x, jump.z);
			RaycastHit2D[] hits = Physics2D.RaycastAll(
					source2D,
					jump2D.normalized,
					jump2D.magnitude + jumperRadius,
					1 << TerrainColliderLayer);
			if (hits.Length > 0)
			{
				var bestHit = hits[0];
				foreach (var hit in hits)
				{
					if (hit.distance > bestHit.distance)
						bestHit = hit;
				}
				return jump * ((bestHit.distance - jumperRadius) / jump2D.magnitude);
			}
			else
				return jump;
		}
	}
}