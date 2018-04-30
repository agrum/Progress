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
		public int numKits;
		public int numClasses;
		public int lengthConstellation;
		public Material abilityMaterial;
		public Material classMaterial;
		public Material kitMaterial;

		private JSONNode constellation = null;
		private List<int> startingAbilityNodeIndexList = new List<int>();
		private List<int> selectedAbilityNodeIndexList = new List<int>();
		private List<int> selectedClassNodeIndexList = new List<int>();
		private List<int> selectedKitNodeIndexList = new List<int>();
		private List<ConstellationNode> abilityNodeList = new List<ConstellationNode>();
		private List<ConstellationNode> classNodeList = new List<ConstellationNode>();
		private List<ConstellationNode> kitNodeList = new List<ConstellationNode>();

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
			Vector2 positionMultiplier = new Vector2();
			positionMultiplier.x = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
			positionMultiplier.y = 0.75f;
			float scale = Math.Min(canvas.pixelRect.width / (2 * (maxX + 1) * positionMultiplier.x), canvas.pixelRect.height / (2 * (maxY + 1) * positionMultiplier.y));
			positionMultiplier.x *= scale;
			positionMultiplier.y *= scale;

			//create constellation nodes
			PopulateNodes(
				abilityArray, 
				abilityMaterial, 
				ConstellationNode.ConstellationNodeType.Ability, 
				scale, 
				positionMultiplier, 
				ref abilityNodeList);
			PopulateNodes(
				constellation["classes"].AsArray, 
				classMaterial, 
				ConstellationNode.ConstellationNodeType.Class, 
				scale, 
				positionMultiplier, 
				ref classNodeList);
			PopulateNodes(
				constellation["kits"].AsArray, 
				kitMaterial, 
				ConstellationNode.ConstellationNodeType.Kit, 
				scale, 
				positionMultiplier, 
				ref kitNodeList);

			SetStartingStateList();

			//connect nodes directly
			JSONArray abilityToAbilityLinkArray = constellation["abilityToAbilityLinks"].AsArray;
			foreach (var abilityToAbilityLink in abilityToAbilityLinkArray)
			{
				JSONArray link = abilityToAbilityLink.Value.AsArray;
				new ConstellationNodeLink(abilityNodeList[link[0].AsInt], abilityNodeList[link[1].AsInt]);
			}
			JSONArray classToAbilityLinkArray = constellation["classToAbilityLinks"].AsArray;
			foreach (var classToAbilityLink in classToAbilityLinkArray)
			{
				JSONArray link = classToAbilityLink.Value.AsArray;
				/*Debug.Log(link);
				Debug.Log(abilityNodeList[link[1].AsInt]);
				Debug.Log(classNodeList[link[0].AsInt]);*/
				abilityNodeList[link[1].AsInt].ClassNodeList.Add(classNodeList[link[0].AsInt]);
			}
			JSONArray kitsToAbilityLinkArray = constellation["kitsToAbilityLinks"].AsArray;
			foreach (var kitsToAbilityLink in kitsToAbilityLinkArray)
			{
				JSONArray link = kitsToAbilityLink.Value.AsArray;
				abilityNodeList[link[1].AsInt].KitsNodeList.Add(kitNodeList[link[0].AsInt]);
			}

			//define the longer links between ability nodes
			foreach (var abilityNode in abilityNodeList)
			{
				abilityNode.DeepPopulateLinks(2);

				for (var i = 0; i < abilityNode.abilityNodeLinkList.Count; ++i)
				{
					if (abilityNode.abilityNodeLinkList[i] == null && i != abilityNode.Index)
					{
						Debug.Log("merp");
					}
				}
			}
		}

		private void SetStartingStateList()
		{
			if (selectedAbilityNodeIndexList.Count != 0)
			{
				Debug.Log("Can't call SetStartingStateList() with nodes preselected");
				return;
			}

			List<bool> abilityStateList = new List<bool>(new bool[abilityNodeList.Count]);
			List<bool> classStateList = new List<bool>(new bool[classNodeList.Count]);
			List<bool> kitStateList = new List<bool>(new bool[kitNodeList.Count]);
			for (int i = 0; i < startingAbilityNodeIndexList.Count; ++i)
				abilityStateList[startingAbilityNodeIndexList[i]] = true;

			SetSelectableStateList(abilityStateList, classStateList, kitStateList);
		}

		private void SetSelectableStateList(List<bool> abilityList, List<bool> classList, List<bool> kitList)
		{
			if (abilityList.Count != abilityNodeList.Count || classList.Count != classNodeList.Count || kitList.Count != kitNodeList.Count)
			{
				Debug.Log("Can't call SetSelectableStateList() with with wrong amount of states to set");
				return;
			}

			for (int i = 0; i < abilityList.Count; ++i)
				abilityNodeList[i].SelectableNode = abilityList[i];
			for (int i = 0; i < classList.Count; ++i)
				classNodeList[i].SelectableNode = classList[i];
			for (int i = 0; i < kitList.Count; ++i)
				kitNodeList[i].SelectableNode = kitList[i];
		}

		private void OnNodeSelected(ConstellationNode node, bool selected)
		{
			if (!node.SelectableNode)
				return;

			//get index of node
			int nodeIndex = node.Index;

			//add or remove from preset
			List<int> selectedNodeIndexList;
			switch(node.Type)
			{
				case ConstellationNode.ConstellationNodeType.Ability:
					selectedNodeIndexList = selectedAbilityNodeIndexList;
					break;
				case ConstellationNode.ConstellationNodeType.Class:
					selectedNodeIndexList = selectedClassNodeIndexList;
					break;
				case ConstellationNode.ConstellationNodeType.Kit:
					selectedNodeIndexList = selectedKitNodeIndexList;
					break;
				default:
					selectedNodeIndexList = selectedAbilityNodeIndexList;
					break;
			}
			if (!selected)
			{
				if (!selectedNodeIndexList.Contains(nodeIndex))
				{
					Debug.Log("Can't call OnNodeSelected() to remove a node that wasn't selected");
					return;
				}

				if (node.Type == ConstellationNode.ConstellationNodeType.Ability)
				{
					if (selectedNodeIndexList[0] == nodeIndex) //clear if it's the initial node
					{
						selectedAbilityNodeIndexList.Clear();
						selectedClassNodeIndexList.Clear();
						selectedKitNodeIndexList.Clear();
					}
					else
					{
						selectedNodeIndexList.Remove(nodeIndex);
						//unselect classes and kits that were solely dependent on this ability
						var newSelectedClassNodeIndexList = new List<int>();
						foreach (var selectedClassNodeIndex in selectedClassNodeIndexList)
						{
							var selectedClassNode = classNodeList[selectedClassNodeIndex];
							foreach (var selectedAbilityNodeIndex in selectedAbilityNodeIndexList)
							{
								if (abilityNodeList[selectedAbilityNodeIndex].ClassNodeList.Contains(selectedClassNode))
								{
									newSelectedClassNodeIndexList.Add(selectedClassNodeIndex);
									break;
								}
							}
						}
						selectedClassNodeIndexList = newSelectedClassNodeIndexList;
						var newSelectedKitNodeIndexList = new List<int>();
						foreach (var selectedKitNodeIndex in selectedKitNodeIndexList)
						{
							var selectedKitNode = kitNodeList[selectedKitNodeIndex];
							foreach (var selectedAbilityNodeIndex in selectedAbilityNodeIndexList)
							{
								if (abilityNodeList[selectedAbilityNodeIndex].ClassNodeList.Contains(selectedKitNode))
								{
									newSelectedKitNodeIndexList.Add(selectedKitNodeIndex);
									break;
								}
							}
						}
						selectedKitNodeIndexList = newSelectedKitNodeIndexList;
					}
				}
				else
					selectedNodeIndexList.Remove(nodeIndex);
			}
			else
			{
				if (selectedNodeIndexList.Contains(nodeIndex))
				{
					Debug.Log("Can't call OnNodeSelected() to add a ndoe already selected");
					return;
				}

				selectedNodeIndexList.Add(nodeIndex);
			}

			//no node selected edge case
			if (selectedAbilityNodeIndexList.Count == 0)
			{
				SetStartingStateList();
				return;
			}

			//create state lists and add selected nodes as true
			List<bool> abilityStateList = new List<bool>(new bool[abilityNodeList.Count]);
			List<bool> classStateList = new List<bool>(new bool[classNodeList.Count]);
			List<bool> kitStateList = new List<bool>(new bool[kitNodeList.Count]);
			for (int i = 0; i < selectedAbilityNodeIndexList.Count; ++i)
				abilityStateList[selectedAbilityNodeIndexList[i]] = true;
			for (int i = 0; i < selectedClassNodeIndexList.Count; ++i)
				classStateList[selectedClassNodeIndexList[i]] = true;
			for (int i = 0; i < selectedKitNodeIndexList.Count; ++i)
				kitStateList[selectedKitNodeIndexList[i]] = true;

			//find selectable ability nodes based on length left
			if (selectedAbilityNodeIndexList.Count < numAbilities)
			{
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
							abilityStateList[linkedNode.Index] = true;
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
						abilityStateList[nodeInRange.Index] = true;
					}
				}
			}

			//find selectable class nodes based on length left
			if (selectedClassNodeIndexList.Count < numClasses)
			{
				foreach (var selectedAbilityNodeIndex in selectedAbilityNodeIndexList)
				{
					foreach (var classNode in abilityNodeList[selectedAbilityNodeIndex].ClassNodeList)
					{
						classStateList[classNode.Index] = true;
					}
				}
			}

			//find selectable kit nodes based on length left
			if (selectedKitNodeIndexList.Count < numKits)
			{
				foreach (var selectedAbilityNodeIndex in selectedAbilityNodeIndexList)
				{
					foreach (var kitNode in abilityNodeList[selectedAbilityNodeIndex].KitsNodeList)
					{
						kitStateList[kitNode.Index] = true;
					}
				}
			}

			SetSelectableStateList(abilityStateList, classStateList, kitStateList);
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

		void PopulateNodes(JSONArray array_, Material nodeMaterial_, ConstellationNode.ConstellationNodeType type_, float scale_, Vector2 positionMultiplier_, ref List<ConstellationNode> nodeList_)
		{
			foreach (var almostNode in array_)
			{
				JSONNode json = almostNode.Value;
				GameObject gob = Instantiate(prefab);
				gob.transform.SetParent(canvas.transform);
				gob.transform.localPosition = new Vector3(json["position"]["x"].AsFloat * positionMultiplier_.x, json["position"]["y"].AsFloat * positionMultiplier_.y, 0);
				gob.transform.localScale = Vector3.one * scale_;
				gob.transform.localRotation = Quaternion.identity;

				ConstellationNode node = gob.GetComponent<ConstellationNode>();
				node.Type = type_;
				node.Mat = nodeMaterial_;
				node.Index = nodeList_.Count;
				node.Uuid = json["id"];
				node.selectedEvent += OnNodeSelected;
				nodeList_.Add(node);
			}
		}
	}
}
