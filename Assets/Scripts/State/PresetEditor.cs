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
	namespace View
	{
		class PresetEditor : MonoBehaviour
		{
			public RectTransform canvas = null;
			public GameObject prefab = null;
			public Material abilityMaterial = null;
			public Material classMaterial = null;
			public Material kitMaterial = null;

			private Model.ConstellationPreset model = null;
			private List<ConstellationNode> abilityNodeList = new List<ConstellationNode>();
			private List<ConstellationNode> classNodeList = new List<ConstellationNode>();
			private List<ConstellationNode> kitNodeList = new List<ConstellationNode>();

			void Start()
			{
				App.Load(() =>
				{
					if (gameObject != null)
						Invoke("Setup", 1.0f);
				});
			}

			private void Setup()
			{
				//return if object died while waiting for answer
				if (!canvas)
					return;

				//TODO take from save instead of made up
				JSONObject presetJson = new JSONObject();
				presetJson["numAbilities"] = 4;
				presetJson["numKits"] = 1;
				presetJson["numClasses"] = 1;
				presetJson["lengthConstellation"] = 8;
				model = new Model.ConstellationPreset(presetJson);
				model.presetUpdateEvent += onPresetUpdate;

				//create ability nodes
				Vector2 positionMultiplier = new Vector2();
				positionMultiplier.x = 0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f);
				positionMultiplier.y = 0.75f;
				float scale = Math.Min(canvas.rect.width / (2 * (App.Constellation.HalfSize.x + 1) * positionMultiplier.x), canvas.rect.height / (2 * (App.Constellation.HalfSize.y + 1) * positionMultiplier.y));
				positionMultiplier.x *= scale;
				positionMultiplier.y *= scale;

				//create constellation nodes
				PopulateNodes(
					App.Constellation.AbilityNodeList,
					abilityMaterial,
					scale,
					positionMultiplier,
					ref abilityNodeList);
				PopulateNodes(
					App.Constellation.ClassNodeList,
					classMaterial,
					scale,
					positionMultiplier,
					ref classNodeList);
				PopulateNodes(
					App.Constellation.KitNodeList,
					kitMaterial,
					scale,
					positionMultiplier,
					ref kitNodeList);

				SetStartingStateList();
			}

			private void SetStartingStateList()
			{
				if (model.SelectedAbilityIndexList.Count != 0)
				{
					Debug.Log("Can't call SetStartingStateList() with nodes preselected");
					return;
				}

				List<bool> abilityStateList = new List<bool>(new bool[App.Constellation.AbilityNodeList.Count]);
				List<bool> classStateList = new List<bool>(new bool[App.Constellation.ClassNodeList.Count]);
				List<bool> kitStateList = new List<bool>(new bool[App.Constellation.KitNodeList.Count]);
				for (int i = 0; i < App.Constellation.StartingAbilityNodeIndexList.Count; ++i)
					abilityStateList[App.Constellation.StartingAbilityNodeIndexList[i]] = true;

				SetSelectableStateList(abilityStateList, classStateList, kitStateList);
			}

			private void SetSelectableStateList(List<bool> abilityList, List<bool> classList, List<bool> kitList)
			{
				if (abilityList.Count != App.Constellation.AbilityNodeList.Count 
					|| classList.Count != App.Constellation.ClassNodeList.Count 
					|| kitList.Count != App.Constellation.KitNodeList.Count)
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

				if (selected)
					model.Add(node.Model);
				else
					model.Remove(node.Model);
			}

			private void onPresetUpdate(Model.ConstellationPreset preset, Model.ConstellationNode node, bool added)
			{ 
				//no node selected edge case
				if (model.SelectedAbilityIndexList.Count == 0)
				{
					SetStartingStateList();
					return;
				}

				//create state lists and add selected nodes as true
				List<bool> abilityStateList = new List<bool>(new bool[abilityNodeList.Count]);
				List<bool> classStateList = new List<bool>(new bool[classNodeList.Count]);
				List<bool> kitStateList = new List<bool>(new bool[kitNodeList.Count]);
				for (int i = 0; i < model.SelectedAbilityIndexList.Count; ++i)
					abilityStateList[model.SelectedAbilityIndexList[i]] = true;
				for (int i = 0; i < model.SelectedClassIndexList.Count; ++i)
					classStateList[model.SelectedClassIndexList[i]] = true;
				for (int i = 0; i < model.SelectedKitIndexList.Count; ++i)
					kitStateList[model.SelectedKitIndexList[i]] = true;

				//find selectable ability nodes based on length left
				if (model.SelectedAbilityIndexList.Count < model.NumAbilities)
				{
					//compute length remaining
					Model.ConstellationNodeLink[,] linkTable = new Model.ConstellationNodeLink[model.SelectedAbilityIndexList.Count, model.SelectedAbilityIndexList.Count];
					for (int i = 0; i < model.SelectedAbilityIndexList.Count; ++i)
					{
						for (int j = i + 1; j < model.SelectedAbilityIndexList.Count; ++j)
						{
							ConstellationNode nodeA = abilityNodeList[model.SelectedAbilityIndexList[i]];
							ConstellationNode nodeB = abilityNodeList[model.SelectedAbilityIndexList[j]];
							Model.ConstellationNodeLink link = nodeA.Model.GetLinkTo(nodeB.Model);

							linkTable[i, j] = link;
							linkTable[j, i] = link;
						}
					}
					int routeIndexUsed = -1;
					int lengthUsed = int.MaxValue;
					List<int> selectedIndexList = new List<int>();
					for (int i = 1; i < model.SelectedAbilityIndexList.Count; ++i)
						selectedIndexList.Add(i);
					List<List<Model.ConstellationNodeLink>> routeList = Model.ConstellationNodeLink.GetRouteList(selectedIndexList, linkTable, 0);
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
					int lengthRemaining = model.LengthConstellation - lengthUsed;

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
					for (int i = 0; i < model.SelectedAbilityIndexList.Count; ++i)
					{
						var selectedNode = abilityNodeList[model.SelectedAbilityIndexList[i]];
						var nodeInRangeList = selectedNode.Model.GetNodeInRangeList(lengthRemaining);
						foreach (var nodeInRange in nodeInRangeList)
						{
							abilityStateList[nodeInRange.Index] = true;
						}
					}
				}

				//find selectable class nodes based on length left
				if (model.SelectedClassIndexList.Count < model.NumClasses)
				{
					foreach (var selectedAbilityNodeIndex in model.SelectedAbilityIndexList)
					{
						foreach (var classNode in App.Constellation.AbilityNodeList[selectedAbilityNodeIndex].ClassNodeList)
						{
							classStateList[classNode.Index] = true;
						}
					}
				}

				//find selectable kit nodes based on length left
				if (model.SelectedKitIndexList.Count < model.NumKits)
				{
					foreach (var selectedAbilityNodeIndex in model.SelectedAbilityIndexList)
					{
						foreach (var kitNode in App.Constellation.AbilityNodeList[selectedAbilityNodeIndex].KitsNodeList)
						{
							kitStateList[kitNode.Index] = true;
						}
					}
				}

				SetSelectableStateList(abilityStateList, classStateList, kitStateList);
			}

			void PopulateNodes(List<Model.ConstellationNode> nodeModelList_, Material nodeMaterial_, float scale_, Vector2 positionMultiplier_, ref List<ConstellationNode> nodeList_)
			{
				foreach (var nodeModel in nodeModelList_)
				{
					GameObject gob = Instantiate(prefab);
					gob.transform.SetParent(canvas.transform);

					ConstellationNode node = gob.GetComponent<ConstellationNode>();
					node.Setup(nodeModel, nodeMaterial_, positionMultiplier_, scale_);
					node.selectedEvent += OnNodeSelected;
					nodeList_.Add(node);
				}
			}
		}
	}
}
