﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace West
{
	namespace View
	{
		public class Constellation : MonoBehaviour
		{
			private Model.ConstellationPreset model = null;
			private RectTransform canvas = null;
			private GameObject prefab = null;
			private Material abilityMaterial = null;
			private Material classMaterial = null;
			private Material kitMaterial = null;

			private List<ConstellationNode> abilityNodeList = new List<ConstellationNode>();
			private List<ConstellationNode> classNodeList = new List<ConstellationNode>();
			private List<ConstellationNode> kitNodeList = new List<ConstellationNode>();

			private Rect lastRect = new Rect();

			public void Setup(
				Model.ConstellationPreset model_,
				GameObject prefab_, 
				Material abilityMaterial_,
				Material classMaterial_, 
				Material kitMaterial_)
			{
				model = model_;
				canvas = GetComponent<RectTransform>();
				prefab = prefab_;
				abilityMaterial = abilityMaterial_;
				classMaterial = classMaterial_;
				kitMaterial = kitMaterial_;
				
				model.presetUpdateEvent += onPresetUpdate;
				
				//create constellation nodes
				PopulateNodes(
					App.Content.Constellation.Model.AbilityNodeList,
					abilityMaterial,
					ref abilityNodeList);
				PopulateNodes(
					App.Content.Constellation.Model.ClassNodeList,
					classMaterial,
					ref classNodeList);
				PopulateNodes(
					App.Content.Constellation.Model.KitNodeList,
					kitMaterial,
					ref kitNodeList);

				SetStartingStateList();
			}

			void Update()
			{
				if (canvas && canvas.rect != lastRect)
				{
					Vector2 positionMultiplier = new Vector2(0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f),  0.75f);
					float scale = Math.Min(
						canvas.rect.width / (2 * (App.Content.Constellation.Model.HalfSize.x + 1) * positionMultiplier.x), 
						canvas.rect.height / (2 * (App.Content.Constellation.Model.HalfSize.y + 1) * positionMultiplier.y));

					foreach (var node in abilityNodeList)
						node.Scale(scale);
					foreach (var node in classNodeList)
						node.Scale(scale);
					foreach (var node in kitNodeList)
						node.Scale(scale);
				}
			}

			private void SetStartingStateList()
			{
				if (model.SelectedAbilityIndexList.Count != 0)
				{
					Debug.Log("Can't call SetStartingStateList() with nodes preselected");
					return;
				}

				List<bool> abilityStateList = new List<bool>(new bool[App.Content.Constellation.Model.AbilityNodeList.Count]);
				List<bool> classStateList = new List<bool>(new bool[App.Content.Constellation.Model.ClassNodeList.Count]);
				List<bool> kitStateList = new List<bool>(new bool[App.Content.Constellation.Model.KitNodeList.Count]);
				for (int i = 0; i < App.Content.Constellation.Model.StartingAbilityNodeIndexList.Count; ++i)
					abilityStateList[App.Content.Constellation.Model.StartingAbilityNodeIndexList[i]] = true;

				SetSelectableStateList(abilityStateList, classStateList, kitStateList);
			}

			private void SetSelectableStateList(List<bool> abilityList, List<bool> classList, List<bool> kitList)
			{
				if (abilityList.Count != App.Content.Constellation.Model.AbilityNodeList.Count
					|| classList.Count != App.Content.Constellation.Model.ClassNodeList.Count
					|| kitList.Count != App.Content.Constellation.Model.KitNodeList.Count)
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
						foreach (var classNode in App.Content.Constellation.Model.AbilityNodeList[selectedAbilityNodeIndex].ClassNodeList)
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
						foreach (var kitNode in App.Content.Constellation.Model.AbilityNodeList[selectedAbilityNodeIndex].KitsNodeList)
						{
							kitStateList[kitNode.Index] = true;
						}
					}
				}

				SetSelectableStateList(abilityStateList, classStateList, kitStateList);
			}

			void PopulateNodes(List<Model.ConstellationNode> nodeModelList_, Material nodeMaterial_, ref List<ConstellationNode> nodeList_)
			{
				foreach (var nodeModel in nodeModelList_)
				{
					GameObject gob = Instantiate(prefab);
					gob.transform.SetParent(canvas.transform);

					ConstellationNode node = gob.GetComponent<ConstellationNode>();
					node.Setup(nodeModel, nodeMaterial_);
					node.selectedEvent += OnNodeSelected;
					nodeList_.Add(node);
				}
			}
		}
	}
}
