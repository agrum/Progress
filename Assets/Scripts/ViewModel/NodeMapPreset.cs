using System;
using System.Collections.Generic;
using UnityEngine;

namespace West.ViewModel
{
	public class NodeMapPreset : INodeMap
	{
		public event OnElementAdded NodeAdded = delegate { };

		private Model.ConstellationPreset model = null;
		private Model.HoveredSkill hoveredModel = null;
		private Model.Json scaleModel = new Model.Json();
		private bool canEdit = false;
		private Material abilityMaterial = null;
		private Material classMaterial = null;
		private Material kitMaterial = null;
			
		private Vector2Int sizeInt = new Vector2Int();
		private Vector2 size = new Vector2();
		private int nodeAdded = 0;

        public NodeMapPreset(
            Model.ConstellationPreset model_,
            Model.HoveredSkill hoveredModel_,
            bool canEdit_)
        {
            Debug.Assert(model_ != null);

            model = model_;
            hoveredModel = hoveredModel_;
            scaleModel["scale"] = 1.0;
            canEdit = canEdit_;
            abilityMaterial = App.Resource.Material.AbilityMaterial;
            classMaterial = App.Resource.Material.ClassMaterial;
            kitMaterial = App.Resource.Material.KitMaterial;

            sizeInt.x = 2;
            sizeInt.y = model.Limits.Ability + model.Limits.Class + model.Limits.Kit;
            //sizeInt.y = sizeInt.y / 2 + sizeInt.y % 2;
            size.x = sizeInt.x;
            size.y = sizeInt.y;
        }

		public void PopulateNodes()
        { 
			PopulateNodes(
				model.Limits.Ability,
				model.Constellation.AbilityNodeList,
				model.SelectedAbilityList,
				Model.Skill.TypeEnum.Ability,
				abilityMaterial);
			PopulateNodes(
                model.Limits.Kit,
				model.Constellation.KitNodeList,
				model.SelectedKitList,
				Model.Skill.TypeEnum.Kit,
				kitMaterial);
			PopulateNodes(
                model.Limits.Class,
				model.Constellation.ClassNodeList,
				model.SelectedClassList,
				Model.Skill.TypeEnum.Class,
				classMaterial);
		}

		~NodeMapPreset()
		{
			NodeAdded = null;
		}

		public void SizeChanged(Rect rect)
		{
			Vector2 positionMultiplier = new Vector2(0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f), 0.75f);
            int numberNodes = model.Limits.Ability + model.Limits.Class + model.Limits.Kit;
            Debug.Log("scale x: " + rect.width / ((size.x) * positionMultiplier.x));
            Debug.Log("scale y: " + rect.height / (numberNodes / 3.0f * (size.y + 0.5f) * positionMultiplier.y));
            scaleModel["scale"] = Math.Min(
				rect.width / ((size.x) * positionMultiplier.x),
				rect.height / (numberNodes / 3.0f * (size.y + 0.5f) * positionMultiplier.y));
		}
			
		private void PopulateNodes(
			int amountNode_,
			List<Model.ConstellationNode> nodeModelList_, 
			List<Model.Skill> selectedSkillList_, 
			Model.Skill.TypeEnum type_,
			Material nodeMaterial_)
		{
			for (int i = 0; i < amountNode_; ++i)
			{
				int localNodeAdded = nodeAdded;
				NodeAdded(() =>
				{
					Vector2 nodePosition = new Vector2(
						-((float)(localNodeAdded % sizeInt.x) - (size.x - 1.0f) / 2.0f),
                        (size.y - 1.0f) / 2.0f - localNodeAdded);
                    Debug.Log(nodePosition);
					return new NodePreset(
						model,
						hoveredModel,
						scaleModel,
						type_,
						i,
						canEdit,
						nodeMaterial_,
						nodePosition);
				});

				++nodeAdded;
			}
		}
	}
}
