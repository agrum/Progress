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
	namespace ViewModel
	{
		public class PresetPreview
		{
			private View.PresetPreview view = null;
			private View.NodeTextualDetails nodeTextualDetails = null;
			private Model.ConstellationPreset model = null;
			private bool canEdit = false;
			private RectTransform canvas = null;
			private GameObject prefab = null;
			private Material abilityMaterial = null;
			private Material classMaterial = null;
			private Material kitMaterial = null;

			private List<NodePreset> abilityNodeList = new List<NodePreset>();
			private List<NodePreset> classNodeList = new List<NodePreset>();
			private List<NodePreset> kitNodeList = new List<NodePreset>();
			
			private Vector2 sizeInt = new Vector2();
			private Vector2 size = new Vector2();
			private int nodeAdded = 0;
			
			public PresetPreview(
				View.PresetPreview viewPresetPreview,
				View.NodeTextualDetails nodeTextualDetails_,
				Model.ConstellationPreset model_,
				bool canEdit_)
			{
				view = viewPresetPreview;
				nodeTextualDetails = nodeTextualDetails_;
				model = model_;
				canEdit = canEdit_;
				canvas = view.GetComponent<RectTransform>();
				prefab = App.Resource.Prefab.ConstellationNode;
				abilityMaterial = App.Resource.Material.AbilityMaterial;
				classMaterial = App.Resource.Material.ClassMaterial;
				kitMaterial = App.Resource.Material.KitMaterial;

				viewPresetPreview.SizeChangedEvent += OnSizeChanged;
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
                if (model != null)
                    model.presetUpdateEvent -= OnPresetUpdate;
            }

			void OnSizeChanged()
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
			
			private void OnPresetUpdate()
			{
				UpdateVisual(abilityNodeList, model.SelectedAbilityList, model.Constellation.AbilityNodeList);
				UpdateVisual(classNodeList, model.SelectedClassList, model.Constellation.ClassNodeList);
				UpdateVisual(kitNodeList, model.SelectedKitList, model.Constellation.KitNodeList);
			}
			
			private void PopulateNodes(
				int amountNode_,
				List<Model.ConstellationNode> nodeModelList_, 
				List<Model.Skill> selectedSkillList_, 
				Material nodeMaterial_, 
				ref List<NodePreset> nodeList_)
			{
				for (int i = 0; i < amountNode_; ++i)
				{
					GameObject gob = view.Add(prefab);

					Vector2 nodePosition = new Vector2(
						-((float) (nodeAdded % sizeInt.x) - (size.x - 1.0f) / 2.0f),
						-(nodeAdded - (size.y + 2.0f) / 2.0f));
					View.Node node = gob.AddComponent<View.Node>();
					nodeList_.Add(new NodePreset(
						node,
						nodeTextualDetails,
						null,
						model,
						canEdit,
						nodeMaterial_,
						nodePosition));
					++nodeAdded;
				}

				UpdateVisual(nodeList_, selectedSkillList_, nodeModelList_);
			}

			delegate Model.ConstellationNode getNodeOutOfSkill(Model.Skill skill_);
			private void UpdateVisual(
				List<NodePreset> nodeViewList_,
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
					nodeViewList_[i].UpdateNode((i < selectedSkillList_.Count) ? lambda(selectedSkillList_[i]) : null);
			}
		}
	}
}
