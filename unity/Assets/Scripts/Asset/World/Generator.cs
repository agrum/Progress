﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace West.Asset.World
{
	public class Generator : MonoBehaviour
	{
		public void DrawEnvironments()
		{
			foreach (var environment in GetComponentsInChildren<Asset.World.Environment>())
			{
				if (environment == null && environment == this)
				{
					continue;
				}
				environment.DrawEnvironmentHierarchy();
			}
		}

        public void Validate()
        {
			if (!Environment.Validate(gameObject))
            {
				return;
            }
		}
	}
}