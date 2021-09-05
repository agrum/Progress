using Assets.Scripts;
using BestHTTP;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace West.Asset.World
{
	public class Generator : MonoBehaviour
	{
		[SerializeField]
		public Environment.EVariety BaselineEnvironment;

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

		public JSONNode ToJson()
        {
			if (!Validate())
            {
				return null;
            }

			bool boundsSet = false;
			Bounds bounds = new Bounds();

			JSONArray environments = new JSONArray();
			foreach (var environment in GetComponentsInChildren<Environment>())
            {
				if (environment == null || environment.transform.parent.gameObject != gameObject)
                {
					continue;
                }

				if (boundsSet)
				{
					var localBounds = environment.Bounds;
					bounds.Encapsulate(localBounds.min);
					bounds.Encapsulate(localBounds.max);
				}
				else
                {
					bounds = environment.Bounds;
					boundsSet = true;
				}
				environments.Add(environment.ToJson());
			}

			JSONArray linearObstacles = new JSONArray();
			foreach (var linearObstacle in GetComponentsInChildren<LinearObstacle>())
			{
				if (linearObstacle == null || linearObstacle.transform.parent.gameObject != gameObject)
				{
					continue;
				}

				if (boundsSet)
				{
					var localBounds = linearObstacle.Bounds;
					bounds.Encapsulate(localBounds.min);
					bounds.Encapsulate(localBounds.max);
				}
				else
				{
					bounds = linearObstacle.Bounds;
					boundsSet = true;
				}
				linearObstacles.Add(linearObstacle.ToJson());
			}

			JSONArray center = new JSONArray();
			center.Add(bounds.center.x);
			center.Add(bounds.center.z);

			JSONArray size = new JSONArray();
			size.Add(bounds.size.x);
			size.Add(bounds.size.z);

			JSONObject main = new JSONObject();
			main["name"] = gameObject.name;
			main["baselineEnvironment"] = BaselineEnvironment.ToString();
			main["environments"] = environments;
			main["linearObstacles"] = linearObstacles;
			main["center"] = center;
			main["size"] = size;

			return main;
		}

		public void Export()
        {
			string jsonString = ToJson().ToString();
			Debug.Log(jsonString);
			var request = App.Server.Request(
				HTTPMethods.Post,
				"tools/outdoorLayoutsUpdate",
				(JSONNode json_) =>
				{
					Debug.Log(json_.ToString());
				});
			request.AddHeader("Content-Type", "application/json");
			request.RawData = System.Text.Encoding.UTF8.GetBytes(jsonString);
			request.Send();
		}
	}
}