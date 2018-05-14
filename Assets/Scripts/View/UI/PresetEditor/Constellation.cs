using System;
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
			public NodeTextualDetails nodeTextualDetails = null;

			private Model.Constellation model = null;
			private Model.ConstellationPreset preset = null;
			private RectTransform canvas = null;
			private GameObject prefab = null;
			private Material abilityMaterial = null;
			private Material classMaterial = null;
			private Material kitMaterial = null;

			private List<ConstellationNode> abilityNodeList = new List<ConstellationNode>();
			private List<ConstellationNode> classNodeList = new List<ConstellationNode>();
			private List<ConstellationNode> kitNodeList = new List<ConstellationNode>();

			private Rect lastRect = new Rect();

			public void Setup(Model.Constellation model_, Model.ConstellationPreset preset_)
			{
				model = model_;
				preset = preset_;
				canvas = GetComponent<RectTransform>();
				prefab = App.Resource.Prefab.ConstellationNode;
				abilityMaterial = App.Resource.Material.AbilityMaterial;
				classMaterial = App.Resource.Material.ClassMaterial;
				kitMaterial = App.Resource.Material.KitMaterial;

				preset.presetUpdateEvent += OnPresetUpdate;
				
				//create constellation nodes
				PopulateNodes(
					model.AbilityNodeList,
					abilityMaterial,
					ref abilityNodeList);
				PopulateNodes(
					model.ClassNodeList,
					classMaterial,
					ref classNodeList);
				PopulateNodes(
					model.KitNodeList,
					kitMaterial,
					ref kitNodeList);

				SetStartingStateList();
            }

            void OnDestroy()
            {
                preset.presetUpdateEvent -= OnPresetUpdate;
                foreach (var node in abilityNodeList)
                {
                    node.selectedEvent -= OnNodeSelected;
                    node.hoveredEvent -= OnNodeHovered;
                }
                foreach (var node in classNodeList)
                {
                    node.selectedEvent -= OnNodeSelected;
                    node.hoveredEvent -= OnNodeHovered;
                }
                foreach (var node in kitNodeList)
                {
                    node.selectedEvent -= OnNodeSelected;
                    node.hoveredEvent -= OnNodeHovered;
                }
            }

            void Update()
			{
				if (canvas && canvas.rect != lastRect)
				{
					Vector2 positionMultiplier = new Vector2(0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f),  0.75f);
					float scale = Math.Min(
						canvas.rect.width / (2 * (model.HalfSize.x + 1) * positionMultiplier.x), 
						canvas.rect.height / (2 * (model.HalfSize.y + 1) * positionMultiplier.y));
					lastRect = new Rect(canvas.rect);

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
				if (preset.SelectedAbilityList.Count != 0)
				{
					Debug.Log("Can't call SetStartingStateList() with nodes preselected");
					return;
				}

				List<ConstellationNode.State> abilityStateList = new List<ConstellationNode.State>(new ConstellationNode.State[model.AbilityNodeList.Count]);
				List<ConstellationNode.State> classStateList = new List<ConstellationNode.State>(new ConstellationNode.State[model.ClassNodeList.Count]);
				List<ConstellationNode.State> kitStateList = new List<ConstellationNode.State>(new ConstellationNode.State[model.KitNodeList.Count]);
				for (int i = 0; i < model.StartingAbilityNodeIndexList.Count; ++i)
					abilityStateList[model.StartingAbilityNodeIndexList[i]] = ConstellationNode.State.Selectable;

				SetSelectableStateList(abilityStateList, classStateList, kitStateList);
			}

			private void SetSelectableStateList(List<ConstellationNode.State> abilityList, List<ConstellationNode.State> classList, List<ConstellationNode.State> kitList)
			{
				if (abilityList.Count != model.AbilityNodeList.Count
					|| classList.Count != model.ClassNodeList.Count
					|| kitList.Count != model.KitNodeList.Count)
				{
					Debug.Log("Can't call SetSelectableStateList() with with wrong amount of states to set");
					return;
				}

				for (int i = 0; i < abilityList.Count; ++i)
					abilityNodeList[i].SelectableState = abilityList[i];
				for (int i = 0; i < classList.Count; ++i)
					classNodeList[i].SelectableState = classList[i];
				for (int i = 0; i < kitList.Count; ++i)
					kitNodeList[i].SelectableState = kitList[i];
			}

			private void OnNodeSelected(ConstellationNode node, bool selected)
			{
				if (node.SelectableState == ConstellationNode.State.Unselectable)
					return;

				if (selected)
					preset.Add(node.Model);
				else
					preset.Remove(node.Model);
			}

			private void OnNodeHovered(ConstellationNode node, bool hovered)
			{
				if (nodeTextualDetails != null)
					nodeTextualDetails.Setup(hovered ? node : null);
			}

			private void OnPresetUpdate()
			{
				//no node selected edge case
				if (preset.SelectedAbilityList.Count == 0)
				{
					SetStartingStateList();
					return;
				}

				//create state lists and add selected nodes as true
				List<ConstellationNode.State> abilityStateList = new List<ConstellationNode.State>(new ConstellationNode.State[abilityNodeList.Count]);
				List<ConstellationNode.State> classStateList = new List<ConstellationNode.State>(new ConstellationNode.State[classNodeList.Count]);
				List<ConstellationNode.State> kitStateList = new List<ConstellationNode.State>(new ConstellationNode.State[kitNodeList.Count]);

				//find selectable ability nodes based on length left
				if (preset.SelectedAbilityList.Count < App.Content.GameSettings.NumAbilities)
				{
					//compute length remaining
					Model.ConstellationNodeLink[,] linkTable = new Model.ConstellationNodeLink[preset.SelectedAbilityList.Count, preset.SelectedAbilityList.Count];
					for (int i = 0; i < preset.SelectedAbilityList.Count; ++i)
					{
						for (int j = i + 1; j < preset.SelectedAbilityList.Count; ++j)
						{
							ConstellationNode nodeA = abilityNodeList[model.AbilityNode(preset.SelectedAbilityList[i]).Index];
							ConstellationNode nodeB = abilityNodeList[model.AbilityNode(preset.SelectedAbilityList[j]).Index];
							Model.ConstellationNodeLink link = nodeA.Model.GetLinkTo(nodeB.Model);

							linkTable[i, j] = link;
							linkTable[j, i] = link;
						}
					}
					int routeIndexUsed = -1;
					int lengthUsed = int.MaxValue;
					List<int> selectedIndexList = new List<int>();
					for (int i = 1; i < preset.SelectedAbilityList.Count; ++i)
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
					int lengthRemaining = App.Content.GameSettings.LengthConstellation - lengthUsed;

					//a fork node might have been removed, elongating routes past the limit.
					//reset in this case.
					if (lengthRemaining < 0)
					{
						preset.Clear();
						return;
					}

					//define selectable on routes
					if (routeIndexUsed != -1)
					{
						foreach (var link in routeList[routeIndexUsed])
						{
							foreach (var linkedNode in link.nodeList)
							{
								abilityStateList[linkedNode.Index] = ConstellationNode.State.Selectable;
							}
						}
					}

					//define selectable and unselectable nodes around selected nodes
					for (int i = 0; i < preset.SelectedAbilityList.Count; ++i)
					{
						var selectedNode = abilityNodeList[model.AbilityNode(preset.SelectedAbilityList[i]).Index];
						var nodeInRangeList = selectedNode.Model.GetNodeInRangeList(lengthRemaining);
						foreach (var nodeInRange in nodeInRangeList)
						{
							abilityStateList[nodeInRange.Index] = ConstellationNode.State.Selectable;
						}
					}
				}

				//find selectable class nodes based on length left
				if (preset.SelectedClassList.Count < App.Content.GameSettings.NumClasses)
				{
					foreach (var selectedAbility in preset.SelectedAbilityList)
					{
						foreach (var classNode in model.AbilityNode(selectedAbility).ClassNodeList)
						{
							classStateList[classNode.Index] = ConstellationNode.State.Selectable;
						}
					}
				}

				//find selectable kit nodes based on length left
				if (preset.SelectedKitList.Count < App.Content.GameSettings.NumKits)
				{
					foreach (var selectedAbility in preset.SelectedAbilityList)
					{
						foreach (var kitNode in model.AbilityNode(selectedAbility).KitsNodeList)
						{
							kitStateList[kitNode.Index] = ConstellationNode.State.Selectable;
						}
					}
				}

				//set selected
				for (int i = 0; i < preset.SelectedAbilityList.Count; ++i)
					abilityStateList[model.AbilityNode(preset.SelectedAbilityList[i]).Index] = ConstellationNode.State.Selected;
				for (int i = 0; i < preset.SelectedClassList.Count; ++i)
					classStateList[model.ClassNode(preset.SelectedClassList[i]).Index] = ConstellationNode.State.Selected;
				for (int i = 0; i < preset.SelectedKitList.Count; ++i)
					kitStateList[model.KitNode(preset.SelectedKitList[i]).Index] = ConstellationNode.State.Selected;

				SetSelectableStateList(abilityStateList, classStateList, kitStateList);
			}

			void PopulateNodes(List<Model.ConstellationNode> nodeModelList_, Material nodeMaterial_, ref List<ConstellationNode> nodeList_)
			{
				foreach (var nodeModel in nodeModelList_)
				{
					GameObject gob = Instantiate(prefab);
					gob.transform.SetParent(canvas.transform);

					ConstellationNode node = gob.AddComponent<ConstellationNode>();
					node.Setup(nodeModel, nodeMaterial_, nodeModel.Position);
					node.selectedEvent += OnNodeSelected;
					node.hoveredEvent += OnNodeHovered;
					nodeList_.Add(node);
				}
			}
		}
	}
}
