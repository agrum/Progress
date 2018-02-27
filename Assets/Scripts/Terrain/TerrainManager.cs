using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	GameObject colliderContainer = null;

	private TerrainManager()
	{
		terrainRoughColliderLayer = LayerMask.NameToLayer("TerrainRoughCollider");
	}

	public int TerrainRoughColliderLayer
	{
		get { return terrainRoughColliderLayer; }
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

	public bool CanJumpTo(Vector3 position)
	{
		Vector2 position2D = new Vector2(position.x, position.z);
		foreach (CircleCollider2D circleCollider in ColliderContainer.GetComponentsInChildren(typeof(CircleCollider2D), true))
		{
			if (circleCollider.gameObject.layer == TerrainRoughColliderLayer)
			{
				if(circleCollider.OverlapPoint(position2D))
				{
					GameObject parent = circleCollider.transform.parent.gameObject;
					PolygonCollider2D[] polygonColliderArray = parent.GetComponentsInChildren<PolygonCollider2D>();
					if (polygonColliderArray.Length > 0 && polygonColliderArray[0].OverlapPoint(position2D))
						return false;
				}
			}
		}

		return true;
	}
}
