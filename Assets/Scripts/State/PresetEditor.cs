using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BestHTTP;
using SimpleJSON;
using UnityEngine.UI;

namespace West
{
	class PresetEditor : MonoBehaviour
	{
		public Canvas canvas;
		public GameObject prefab;

		private JSONNode constellation = null;
		private List<int> startingAbilityIndexList = new List<int>();
		private List<ConstellationNode> abilityNodeList = new List<ConstellationNode>();
		private List<ConstellationNode> startingAbilityNodeList = new List<ConstellationNode>();

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
			JSONArray startingAbilityIndexArray = constellation["startingAbilities"].AsArray;

			//find starting node indexes
			foreach (var startingAbilityindex in startingAbilityIndexArray)
			{
				startingAbilityIndexList.Add(startingAbilityindex.Value.AsInt);
			}

			//find scale factor
			float maxX = 0;
			float maxY = 0;
			JSONArray abilityArray = constellation["abilities"].AsArray;
			foreach (var abilityNode in abilityArray)
				{
				JSONNode ability = abilityNode.Value;
				if (Math.Abs(ability["position"]["x"].AsFloat) > maxX)
					maxX = Math.Abs(ability["position"]["x"].AsFloat);
				if (Math.Abs(ability["position"]["y"].AsFloat) > maxY)
					maxY = Math.Abs(ability["position"]["y"].AsFloat);
			}

			//create nodes
			float xMultiplier = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
			float yMultiplier = 0.75f;
			float scale = Math.Min(canvas.pixelRect.width / (2 * (maxX + 1) * xMultiplier), canvas.pixelRect.height / (2 * (maxY + 1) * yMultiplier));
			xMultiplier *= scale;
			yMultiplier *= scale;
			foreach (var abilityNode in abilityArray)
			{
				JSONNode ability = abilityNode.Value;
				GameObject gob = Instantiate(prefab);
				gob.transform.SetParent(canvas.transform);
				gob.transform.localPosition = new Vector3(ability["position"]["x"].AsFloat * xMultiplier, ability["position"]["y"].AsFloat * yMultiplier, 0);
				gob.transform.localScale = Vector3.one * scale;
				gob.transform.localRotation = Quaternion.identity;

				ConstellationNode node = gob.GetComponent<ConstellationNode>();
				node.Setup();
				node.selectedEvent += OnNodeSelected;
				if (startingAbilityIndexList.Contains(abilityNodeList.Count))
				{
					node.SelectableNode = true;
					startingAbilityNodeList.Add(node);
				}
				abilityNodeList.Add(node);
			}

			//connect nodes
			JSONArray abilityToAbilityLinkArray = constellation["abilityToAbilityLinks"].AsArray;
			foreach (var abilityToAbilityLink in abilityToAbilityLinkArray)
			{
				JSONArray link = abilityToAbilityLink.Value.AsArray;
				ConstellationNode.Link(abilityNodeList[link[0].AsInt], abilityNodeList[link[1].AsInt]);
			}
		}

		private void OnNodeSelected(ConstellationNode node)
		{
			if (!node.SelectableNode)
				return;

			node.SelectNode();
		}
	}
}
