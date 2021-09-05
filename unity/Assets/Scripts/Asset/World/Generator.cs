using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace West.Asset.World
{
	public class Generator : MonoBehaviour
	{
        public void Draw()
        {
			DrawEnvironments();
			DrawLinearObstacles();
		}

        public void DrawEnvironments()
		{
			foreach (var environment in GetComponentsInChildren<Environment>())
			{
				if (environment == null && environment == this)
				{
					continue;
				}
				environment.DrawHierarchy();
			}
		}
		public void DrawLinearObstacles()
		{
			foreach (var linearObstacle in GetComponentsInChildren<LinearObstacle>())
			{
				if (linearObstacle == null && linearObstacle == this)
				{
					continue;
				}
				linearObstacle.DrawHierarchy();
			}
		}

		public bool Validate()
		{
			if (!Environment.Validate(gameObject))
			{
				return false;
			}

			foreach (var linearObstacle in GetComponentsInChildren<LinearObstacle>())
			{
				if (!linearObstacle.Validate())
				{
					return false;
				}
			}

			return true;
		}
	}
}