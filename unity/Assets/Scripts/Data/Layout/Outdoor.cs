using Assets.Scripts;
using BestHTTP;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Data.Layout
{
	public class Outdoor
	{
		public string Name;
		public Environment.EVariety BaselineEnvironment;
		public List<Environment> Environments = new List<Environment>();
		public List<LinearObstacle> LinearObstacles = new List<LinearObstacle>();
		public Vector2 Center;
		public Vector2 Size;

		public Outdoor(JSONNode node)
		{
			Name = node["name"];
			BaselineEnvironment = Data.Serializer.ReadEnum<Environment.EVariety>(node["baselineEnvironment"]);
			foreach (var environment in node["environments"].AsArray)
			{
				Environments.Add(new Environment(environment));
			}
			foreach (var linearObstacle in node["linearObstacles"].AsArray)
			{
				LinearObstacles.Add(new LinearObstacle(linearObstacle));
			}
			Center.x = node["center"].AsArray[0];
			Center.y = node["center"].AsArray[1];
			Size.x = node["size"].AsArray[0];
			Size.y = node["size"].AsArray[1];
		}

		private Outdoor(Outdoor other_)
        {
			Name = other_.Name;
			BaselineEnvironment = other_.BaselineEnvironment;
			foreach (var environment in other_.Environments)
			{
				Environments.Add(new Environment(environment));
			}
			foreach (var linearObstacle in other_.LinearObstacles)
			{
				LinearObstacles.Add(new LinearObstacle(linearObstacle));
			}
			Center = other_.Center;
			Size = other_.Size;
		}

		public Outdoor Fractured()
        {
			return new Outdoor(this);
        }
	}
}