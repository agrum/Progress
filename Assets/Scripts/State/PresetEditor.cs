using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BestHTTP;
using SimpleJSON;

namespace West
{
	class PresetEditor : MonoBehaviour
	{
		public Canvas canvas;
		public GameObject prefab;

		private JSONNode constellation = null;

		void Start()
		{
			App.Load(()=>
			{
				App.Request(
					HTTPMethods.Get,
					"constellation/" + App.Model["constellation"],
					(JSONNode json) => {
						OnConstellationRequestFinished(json);
					})
					.Send();
			});
		}

		private void OnConstellationRequestFinished(JSONNode json)
		{
			//return if object died while waiting for answer
			if (!canvas)
				return;

			constellation = json;
			JSONArray abilities = constellation["abilities"].AsArray;

			//fix max coordinates
			float maxX = 0;
			float maxY = 0;
			foreach (var abilityNode in abilities)
			{
				JSONNode ability = abilityNode.Value;
				GameObject node = Instantiate(prefab);
				if (Math.Abs(ability["position"]["x"].AsFloat) > maxX)
					maxX = Math.Abs(ability["position"]["x"].AsFloat);
				if (Math.Abs(ability["position"]["y"].AsFloat) > maxY)
					maxY = Math.Abs(ability["position"]["y"].AsFloat);
			}

			//create nodes
			float xMultiplier = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
			float yMultiplier = 0.75f;
			float scale = Math.Min(canvas.pixelRect.width / (2 * (maxX+1) * xMultiplier), canvas.pixelRect.height / (2 * (maxY+1) * yMultiplier));
			xMultiplier *= scale;
			yMultiplier *= scale;
			foreach (var abilityNode in abilities)
			{
				JSONNode ability = abilityNode.Value;
				GameObject node = Instantiate(prefab);
				node.transform.SetParent(canvas.transform);
				node.transform.localPosition = new Vector3(ability["position"]["x"].AsFloat * xMultiplier, ability["position"]["y"].AsFloat * yMultiplier, 0);
				node.transform.localScale = Vector3.one * scale;
				node.transform.localRotation = Quaternion.identity;
			}
		}
	}
}
