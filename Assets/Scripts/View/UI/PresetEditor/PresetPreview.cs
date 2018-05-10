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
		public class PresetPreview : MonoBehaviour
		{
			public NodeTextualDetails nodeTextualDetails = null;

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
				PopulateNodes(
					model.NumAbilities,
					App.Content.Constellation.Model.AbilityNodeList,
					model.SelectedAbilityIndexList,
					abilityMaterial,
					ref abilityNodeList);
				PopulateNodes(
					model.NumKits,
					App.Content.Constellation.Model.KitNodeList,
					model.SelectedKitIndexList,
					kitMaterial,
					ref kitNodeList);
				PopulateNodes(
					model.NumClasses,
					App.Content.Constellation.Model.ClassNodeList,
					model.SelectedClassIndexList,
					classMaterial,
					ref classNodeList);
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
				UpdateVisual(abilityNodeList, model.SelectedAbilityIndexList, App.Content.Constellation.Model.AbilityNodeList);
				UpdateVisual(classNodeList, model.SelectedClassIndexList, App.Content.Constellation.Model.ClassNodeList);
				UpdateVisual(kitNodeList, model.SelectedKitIndexList, App.Content.Constellation.Model.KitNodeList);
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
				List<int> nodeIndexList_, 
				Material nodeMaterial_, 
				ref List<ConstellationNode> nodeList_)
			{
				List<Model.ConstellationNode> nodeModelSubsetList = new List<Model.ConstellationNode>(model.NumClasses);
				for (int i = 0; i < amountNode_; ++i)
					nodeModelSubsetList.Add((i < nodeIndexList_.Count)
						? nodeModelList_[nodeIndexList_[i]]
						: null);

				foreach (var nodeModel in nodeModelSubsetList)
				{
					GameObject gob = Instantiate(prefab);
					gob.transform.SetParent(canvas.transform);

					Vector2 nodePosition = new Vector2(
						-((float) (nodeAdded % sizeInt.x) - (size.x - 1.0f) / 2.0f),
						-(nodeAdded - (size.y + 2.0f) / 2.0f));
					ConstellationNode node = gob.AddComponent<ConstellationNode>();
					node.Setup(nodeModel, nodeMaterial_, nodePosition);
					node.selectedEvent += OnNodeSelected;
					node.hoveredEvent += OnNodeHovered;
					nodeList_.Add(node);
					++nodeAdded;
				}
			}

			private void UpdateVisual(
				List<ConstellationNode> nodeViewList,
				List<int> nodeIndexlist,
				List<Model.ConstellationNode> nodeModelList)
			{
				for (int i = 0; i < nodeViewList.Count; ++i)
				{
					if (i < nodeIndexlist.Count)
					{
						nodeViewList[i].Setup(nodeModelList[nodeIndexlist[i]]);
						nodeViewList[i].SelectableState = ConstellationNode.State.Selected;
					}
					else
						nodeViewList[i].Setup(null);
				}
			}
		}
	}
}
