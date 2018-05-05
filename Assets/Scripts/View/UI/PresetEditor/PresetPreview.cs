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

				model.presetUpdateEvent += OnPresetUpdate;

				sizeInt.x = 2;
				sizeInt.y = model.NumAbilities + model.NumClasses + model.NumKits;
				sizeInt.y = sizeInt.y / 2 + sizeInt.y % 2;
				size.x = sizeInt.x;
				size.y = sizeInt.y;

				//create constellation nodes
				List<Model.ConstellationNode> modelAbilityNodeList = new List<Model.ConstellationNode>(model.NumAbilities);
				for (int i = 0; i < model.NumAbilities; ++i)
					modelAbilityNodeList.Add((i < model.SelectedAbilityIndexList.Count) 
						? App.Content.Constellation.Model.AbilityNodeList[model.SelectedAbilityIndexList[i]]
						: null);
				PopulateNodes(
					modelAbilityNodeList,
					abilityMaterial,
					ref abilityNodeList);
				List<Model.ConstellationNode> modelKitNodeList = new List<Model.ConstellationNode>(model.NumKits);
				for (int i = 0; i < model.NumKits; ++i)
					modelKitNodeList.Add((i < model.SelectedKitIndexList.Count)
						? App.Content.Constellation.Model.KitNodeList[model.SelectedKitIndexList[i]]
						: null);
				PopulateNodes(
					modelKitNodeList,
					kitMaterial,
					ref kitNodeList);
				List<Model.ConstellationNode> modelClassNodeList = new List<Model.ConstellationNode>(model.NumClasses);
				for (int i = 0; i < model.NumClasses; ++i)
					modelClassNodeList.Add((i < model.SelectedClassIndexList.Count)
						? App.Content.Constellation.Model.ClassNodeList[model.SelectedClassIndexList[i]]
						: null);
				PopulateNodes(
					modelClassNodeList,
					classMaterial,
					ref classNodeList);
			}

			void Update()
			{
				if (canvas && canvas.rect != lastRect)
				{
					Vector2 positionMultiplier = new Vector2(0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f), 0.75f);
					float scale = Math.Min(
						canvas.rect.width / ((size.x + 1.0f) * positionMultiplier.x),
						canvas.rect.height / (2.0f * (size.y + 1.0f) * positionMultiplier.y));

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
				for (int i = 0; i < abilityNodeList.Count; ++i)
				{
					if (i < model.SelectedAbilityIndexList.Count)
					{
						abilityNodeList[i].Setup(App.Content.Constellation.Model.AbilityNodeList[model.SelectedAbilityIndexList[i]]);
						abilityNodeList[i].SelectNode(true);
					}
					else
						abilityNodeList[i].Setup(null);
				}
				for (int i = 0; i < classNodeList.Count; ++i)
				{
					if (i < model.SelectedClassIndexList.Count)
					{
						classNodeList[i].Setup(App.Content.Constellation.Model.ClassNodeList[model.SelectedClassIndexList[i]]);
						classNodeList[i].SelectNode(true);
					}
					else
						classNodeList[i].Setup(null);
				}
				for (int i = 0; i < kitNodeList.Count; ++i)
				{
					if (i < model.SelectedKitIndexList.Count)
					{
						kitNodeList[i].Setup(App.Content.Constellation.Model.KitNodeList[model.SelectedKitIndexList[i]]);
						kitNodeList[i].SelectNode(true);
					}
					else
						kitNodeList[i].Setup(null);
				}
			}

			private void OnNodeSelected(ConstellationNode node, bool selected)
			{
			}

			void PopulateNodes(List<Model.ConstellationNode> nodeModelList_, Material nodeMaterial_, ref List<ConstellationNode> nodeList_)
			{
				foreach (var nodeModel in nodeModelList_)
				{
					GameObject gob = Instantiate(prefab);
					gob.transform.SetParent(canvas.transform);

					Vector2 nodePosition = new Vector2(
						-((float) (nodeAdded % sizeInt.x) - (size.x - 1.0f) / 2.0f),
						-(nodeAdded - (size.y + 2.0f) / 2.0f));
					ConstellationNode node = gob.AddComponent<ConstellationNode>();
					node.Setup(nodeModel, nodeMaterial_, nodePosition);
					node.selectedEvent += OnNodeSelected;
					nodeList_.Add(node);
					++nodeAdded;
				}
			}
		}
	}
}
