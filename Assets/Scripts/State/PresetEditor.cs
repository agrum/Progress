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
		public Material abilityMaterial;
		public Material classMaterial;
		public Material kitMaterial;

		private JSONNode constellation = null;
		private List<int> startingAbilityNodeIndexList = new List<int>();
		private List<int> selectedAbilityNodeIndexList = new List<int>();
		private List<ConstellationNode> abilityNodeList = new List<ConstellationNode>();
		private List<ConstellationNode> classNodeList = new List<ConstellationNode>();

		void Start()
		{
			App.Load(() =>
			{
				App.Request(
					HTTPMethods.Get,
					"constellation/" + App.Model["constellation"],
					(JSONNode json) =>
					{
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

			//create ability nodes
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
				gob.transform.Find("GameObject").Find("HexagonStroke").GetComponent<Image>().material = abilityMaterial;
				node.Type = ConstellationNode.ConstellationNodeType.Ability;
				node.Index = abilityNodeList.Count;
				node.Uuid = ability["id"];
				node.selectedEvent += OnNodeSelected;
				abilityNodeList.Add(node);
			}

			//create classes nodes
			JSONArray classArray = constellation["classes"].AsArray;
			foreach (var classNode in classArray)
			{
				JSONNode class_ = classNode.Value;
				GameObject gob = Instantiate(prefab);
				gob.transform.SetParent(canvas.transform);
				gob.transform.localPosition = new Vector3(class_["position"]["x"].AsFloat * xMultiplier, class_["position"]["y"].AsFloat * yMultiplier, 0);
				gob.transform.localScale = Vector3.one * scale;
				gob.transform.localRotation = Quaternion.identity;

				ConstellationNode node = gob.GetComponent<ConstellationNode>();
				gob.transform.Find("GameObject").Find("HexagonStroke").GetComponent<Image>().material = classMaterial;
				node.Type = ConstellationNode.ConstellationNodeType.Class;
				node.Index = classNodeList.Count;
				node.Uuid = class_["id"];
				//node.selectedEvent += OnNodeSelected;
				classNodeList.Add(node);
			}

			SetStartingStateList();

			//connect nodes directly
			JSONArray abilityToAbilityLinkArray = constellation["abilityToAbilityLinks"].AsArray;
			foreach (var abilityToAbilityLink in abilityToAbilityLinkArray)
			{
				JSONArray link = abilityToAbilityLink.Value.AsArray;
				new ConstellationNodeLink(abilityNodeList[link[0].AsInt], abilityNodeList[link[1].AsInt]);
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
			int nodeIndex = node.Index;

			//add or remove from preset
			if (!selected)
			{
				if (!selectedAbilityNodeIndexList.Contains(nodeIndex))
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
			if (selectedAbilityNodeIndexList.Count == 0)
			{
				SetStartingStateList();
				return;
			}

			//create state list and add selected nodes as true
			List<bool> stateList = new List<bool>(new bool[abilityNodeList.Count]);
			for (int i = 0; i < selectedAbilityNodeIndexList.Count; ++i)
				stateList[selectedAbilityNodeIndexList[i]] = true;

			//set complete edge case
			if (selectedAbilityNodeIndexList.Count == numAbilities)
			{
				SetSelectableStateList(stateList);
				return;
			}

			//compute length remaining
			ConstellationNodeLink[,] linkTable = new ConstellationNodeLink[selectedAbilityNodeIndexList.Count, selectedAbilityNodeIndexList.Count];
			for (int i = 0; i < selectedAbilityNodeIndexList.Count; ++i)
			{
				for (int j = i + 1; j < selectedAbilityNodeIndexList.Count; ++j)
				{
					ConstellationNode nodeA = abilityNodeList[selectedAbilityNodeIndexList[i]];
					ConstellationNode nodeB = abilityNodeList[selectedAbilityNodeIndexList[j]];
					ConstellationNodeLink link = nodeA.GetLinkTo(nodeB);
					linkTable[i, j] = link;
					linkTable[j, i] = link;
				}
			}
			int routeIndexUsed = -1;
			int lengthUsed = int.MaxValue;
			List<int> selectedIndexList = new List<int>();
			for (int i = 1; i < selectedAbilityNodeIndexList.Count; ++i)
				selectedIndexList.Add(i);
			List<List<ConstellationNodeLink>> routeList = GetRouteList(selectedIndexList, linkTable, 0);
			if (routeList.Count == 0)
				lengthUsed = 0;
			for (int i = 0; i < routeList.Count; ++i)
			{
				int routeLength = 0;
				foreach (var link in routeList[i])
				{
					routeLength += link.Depth;
				}
				if (routeLength < lengthUsed)
				{
					routeIndexUsed = i;
					lengthUsed = routeLength;
				}
			}
			int lengthRemaining = lengthConstellation - lengthUsed;

			//define selectable on routes
			if (routeIndexUsed != -1)
			{
				foreach (var link in routeList[routeIndexUsed])
				{
					foreach (var linkedNode in link.nodeList)
					{
						stateList[linkedNode.Index] = true;
					}
				}
			}

			//define selectable and unselectable nodes around selected nodes
			for (int i = 0; i < selectedAbilityNodeIndexList.Count; ++i)
			{
				var selectedNode = abilityNodeList[selectedAbilityNodeIndexList[i]];
				var nodeInRangeList = selectedNode.GetNodeInRangeList(lengthRemaining);
				foreach (var nodeInRange in nodeInRangeList)
				{
					stateList[nodeInRange.Index] = true;
				}
			}
			SetSelectableStateList(stateList);
		}

		List<List<ConstellationNodeLink>> GetRouteList(List<int> remainIndexList, ConstellationNodeLink[,] linkTable, int sourceIndex)
		{
			List<List<ConstellationNodeLink>> routeList = new List<List<ConstellationNodeLink>>();

			if (remainIndexList.Count == 0)
				return routeList;

			for (int i = 0; i < remainIndexList.Count; ++i)
			{
				ConstellationNodeLink link = linkTable[sourceIndex, remainIndexList[i]];
				List<List<ConstellationNodeLink>> subRouteList = new List<List<ConstellationNodeLink>>();

				if (remainIndexList.Count > 1)
				{
					List<int> subRemainIndexList = new List<int>(remainIndexList);
					subRemainIndexList.RemoveAt(i);

					subRouteList.AddRange(GetRouteList(subRemainIndexList, linkTable, sourceIndex));
					subRouteList.AddRange(GetRouteList(subRemainIndexList, linkTable, remainIndexList[i]));
					for (int j = 0; j < subRouteList.Count; ++j)
						subRouteList[j].Add(link);
				}
				else
				{
					subRouteList.Add(new List<ConstellationNodeLink>());
					subRouteList[0].Add(link);
				}

				routeList.AddRange(subRouteList);
			}

			return routeList;
		}
	}
}
