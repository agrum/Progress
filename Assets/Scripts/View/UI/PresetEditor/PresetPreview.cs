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
		public class PresetPreview : MonoBehaviour
		{
			private Model.ConstellationPreset model = null;
			private NodeTextualDetails nodeTextualDetails = null;
			private RectTransform canvas = null;
			private GameObject prefab = null;
			private Material abilityMaterial = null;
			private Material classMaterial = null;
			private Material kitMaterial = null;

			private List<ConstellationNode> abilityNodeList = new List<ConstellationNode>();
			private List<ConstellationNode> classNodeList = new List<ConstellationNode>();
			private List<ConstellationNode> kitNodeList = new List<ConstellationNode>();

			private Rect lastRect = new Rect();
			private Vector2 sizeInt = new Vector2();
			private Vector2 size = new Vector2();
			private int nodeAdded = 0;
			
			public void Setup(Model.ConstellationPreset model_, NodeTextualDetails nodeTextualDetails_)
			{
				model = model_;
				nodeTextualDetails = nodeTextualDetails_;
				canvas = GetComponent<RectTransform>();
				prefab = App.Resource.Prefab.ConstellationNode;
				abilityMaterial = App.Resource.Material.AbilityMaterial;
				classMaterial = App.Resource.Material.ClassMaterial;
				kitMaterial = App.Resource.Material.KitMaterial;

				model.presetUpdateEvent += OnPresetUpdate;

				sizeInt.x = 2;
				sizeInt.y = App.Content.GameSettings.NumAbilities + App.Content.GameSettings.NumClasses + App.Content.GameSettings.NumKits;
				sizeInt.y = sizeInt.y / 2 + sizeInt.y % 2;
				size.x = sizeInt.x;
				size.y = sizeInt.y;

				//create constellation nodes
				PopulateNodes(
					App.Content.GameSettings.NumAbilities,
					model.Constellation.AbilityNodeList,
					model.SelectedAbilityList,
					abilityMaterial,
					ref abilityNodeList);
				PopulateNodes(
					App.Content.GameSettings.NumKits,
					model.Constellation.KitNodeList,
					model.SelectedKitList,
					kitMaterial,
					ref kitNodeList);
				PopulateNodes(
					App.Content.GameSettings.NumClasses,
					model.Constellation.ClassNodeList,
					model.SelectedClassList,
					classMaterial,
					ref classNodeList);
			}

            void OnDestroy()
            {
                model.presetUpdateEvent -= OnPresetUpdate;
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
					Vector2 positionMultiplier = new Vector2(0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f), 0.75f);
					float scale = Math.Min(
						canvas.rect.width / ((size.x) * positionMultiplier.x),
						canvas.rect.height / (2.0f * (size.y + 0.5f) * positionMultiplier.y));

					foreach (var node in abilityNodeList)
						node.Scale(scale);
					foreach (var node in classNodeList)
						node.Scale(scale);
					foreach (var node in kitNodeList)
						node.Scale(scale);
				}
			}
			
			private void OnPresetUpdate()
			{
				UpdateVisual(abilityNodeList, model.SelectedAbilityList, model.Constellation.AbilityNodeList);
				UpdateVisual(classNodeList, model.SelectedClassList, model.Constellation.ClassNodeList);
				UpdateVisual(kitNodeList, model.SelectedKitList, model.Constellation.KitNodeList);
			}

			private void OnNodeSelected(ConstellationNode node, bool selected)
			{
				if (node.Model != null && !selected)
					model.Remove(node.Model);
			}

			private void OnNodeHovered(ConstellationNode node, bool hovered)
			{
				if (nodeTextualDetails != null && node.Model != null)
					nodeTextualDetails.Setup(hovered ? node : null);
			}
			
			private void PopulateNodes(
				int amountNode_,
				List<Model.ConstellationNode> nodeModelList_, 
				List<Model.Skill> selectedSkillList_, 
				Material nodeMaterial_, 
				ref List<ConstellationNode> nodeList_)
			{
				for (int i = 0; i < amountNode_; ++i)
				{
					GameObject gob = Instantiate(prefab);
					gob.transform.SetParent(canvas.transform);

					Vector2 nodePosition = new Vector2(
						-((float) (nodeAdded % sizeInt.x) - (size.x - 1.0f) / 2.0f),
						-(nodeAdded - (size.y + 2.0f) / 2.0f));
					ConstellationNode node = gob.AddComponent<ConstellationNode>();
					node.Setup(null, nodeMaterial_, nodePosition);
					node.selectedEvent += OnNodeSelected;
					node.hoveredEvent += OnNodeHovered;
					nodeList_.Add(node);
					++nodeAdded;
				}

				UpdateVisual(nodeList_, selectedSkillList_, nodeModelList_);
			}

			delegate Model.ConstellationNode getNodeOutOfSkill(Model.Skill skill_);
			private void UpdateVisual(
				List<ConstellationNode> nodeViewList_,
				List<Model.Skill> selectedSkillList_,
				List<Model.ConstellationNode> nodeModelList_)
			{
				getNodeOutOfSkill lambda = (Model.Skill skill_) =>
				{
					foreach (var nodeModel in nodeModelList_)
						if (nodeModel.Skill == skill_)
							return nodeModel;

					return null;
				};

				for (int i = 0; i < nodeViewList_.Count; ++i)
				{
					if (i < selectedSkillList_.Count)
					{
						nodeViewList_[i].Setup(lambda(selectedSkillList_[i]));
						nodeViewList_[i].SelectableState = ConstellationNode.State.Selected;
					}
					else
						nodeViewList_[i].Setup(null);
				}
			}
		}
	}
}
