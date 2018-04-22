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
		public int numAbilities;
		public int lengthConstellation;

		private JSONNode constellation = null;
		private List<int> startingAbilityNodeIndexList = new List<int>();
		private List<int> selectedAbilityNodeIndexList = new List<int>();
		private List<ConstellationNode> abilityNodeList = new List<ConstellationNode>();

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
				startingAbilityNodeIndexList.Add(startingAbilityindex.Value.AsInt);
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
				node.Index = abilityNodeList.Count;
				node.selectedEvent += OnNodeSelected;
				abilityNodeList.Add(node);
			}

			SetStartingStateList();

			//connect nodes directly
			JSONArray abilityToAbilityLinkArray = constellation["abilityToAbilityLinks"].AsArray;
			foreach (var abilityToAbilityLink in abilityToAbilityLinkArray)
			{
				JSONArray link = abilityToAbilityLink.Value.AsArray;
				ConstellationNode.Link(abilityNodeList[link[0].AsInt], abilityNodeList[link[1].AsInt], 1);
			}

			//define the longer links between nodes
			foreach (var abilityNode in abilityNodeList)
			{
				abilityNode.DeepPopulateLinks(2);
			}
		}

		private void SetStartingStateList()
		{
			if (selectedAbilityNodeIndexList.Count != 0)
			{
				Debug.Log("Can't call SetStartingStateList() with nodes preselected");
				return;
			}

			List<bool> stateList = new List<bool>(new bool[abilityNodeList.Count]);
			for (int i = 0; i < startingAbilityNodeIndexList.Count; ++i)
				stateList[startingAbilityNodeIndexList[i]] = true;

			SetSelectableStateList(stateList);
		}

		private void SetSelectableStateList(List<bool> list)
		{
			if (list.Count != abilityNodeList.Count)
			{
				Debug.Log("Can't call SetSelectableStateList() with with wrong amount of states to set");
				return;
			}

			for (int i = 0; i < list.Count; ++i)
				abilityNodeList[i].SelectableNode = list[i];
		}

		private void OnNodeSelected(ConstellationNode node, bool selected)
		{
			if (!node.SelectableNode)
				return;

			//get index of node
			int nodeIndex = 0;

			//add or remove from preset
			if (!selected)
			{
				if(!selectedAbilityNodeIndexList.Contains(nodeIndex))
				{
					Debug.Log("Can't call OnNodeSelected() to remove a node that wasn't selected");
					return;
				}

				if (selectedAbilityNodeIndexList[0] == nodeIndex) //clear if it's the initial node
					selectedAbilityNodeIndexList.Clear();
				else
					selectedAbilityNodeIndexList.Remove(nodeIndex);
			}
			else
			{
				if (selectedAbilityNodeIndexList.Contains(nodeIndex))
				{
					Debug.Log("Can't call OnNodeSelected() to add a ndoe already selected");
					return;
				}

				selectedAbilityNodeIndexList.Add(nodeIndex);
			}

			//no node selected edge case
			if(selectedAbilityNodeIndexList.Count == 0)
			{
				SetStartingStateList();
				return;
			}

			//create state list and add selected nodes as true
			List<bool> stateList = new List<bool>(new bool[abilityNodeList.Count]);
			for (int i = 0; i < selectedAbilityNodeIndexList.Count; ++i)
				abilityNodeList[selectedAbilityNodeIndexList[i]].SelectableNode = true;

			//set complete edge case
			if (selectedAbilityNodeIndexList.Count == numAbilities)
			{
				SetSelectableStateList(stateList);
				return;
			}

			//compute length remaining

			//define selectable and unselectable nodes
		}
	}
}
