using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ViewModel
{
	public class NodeMapConstellation : INodeMap
	{
		public event OnElementAdded NodeAdded = delegate { };

		private Model.Constellation model = null;
		private Model.ConstellationPreset preset = null;
		private Model.HoveredSkill hovered = null;
		private Model.Json scaleModel = new Model.Json();

        private List<Data.Skill.Skill> filteredAbilityList = new List<Data.Skill.Skill>();
        private List<Data.Skill.Skill> filteredClassList = new List<Data.Skill.Skill>();
        private List<Data.Skill.Skill> filteredKitList = new List<Data.Skill.Skill>();

        private Material abilityMaterial = null;
		private Material classMaterial = null;
		private Material kitMaterial = null;

        public NodeMapConstellation(
            Model.Constellation model_,
            Model.ConstellationPreset preset_,
            List<Data.Skill.Skill> filtererSkills_,
            Model.HoveredSkill hovered_)
        {
            Debug.Assert(model_ != null);
            Debug.Assert(preset_ != null);
            Debug.Assert(hovered_ != null);

            model = model_;
            preset = preset_;
            hovered = hovered_;

            scaleModel["scale"] = 1.0;
            abilityMaterial = App.Resource.Material.AbilityMaterial;
            classMaterial = App.Resource.Material.ClassMaterial;
            kitMaterial = App.Resource.Material.KitMaterial;

            foreach (var skill in filtererSkills_)
            {
                switch (skill.Category)
                {
                    case Data.Skill.Skill.ECategory.Ability:
                        filteredAbilityList.Add(skill);
                        break;
                    case Data.Skill.Skill.ECategory.Class:
                        filteredClassList.Add(skill);
                        break;
                    case Data.Skill.Skill.ECategory.Kit:
                        filteredKitList.Add(skill);
                        break;
                    default:
                        Debug.Log("NodeMapPreset() weird filter given");
                        break;
                }
            }
        }

        public void PopulateNodes()
        {
            PopulateNodes(
				model.AbilityNodeList,
                filteredAbilityList,
                abilityMaterial);
			PopulateNodes(
				model.ClassNodeList,
                filteredClassList,
                classMaterial);
			PopulateNodes(
				model.KitNodeList,
                filteredKitList,
                kitMaterial);
		}

		public void SizeChanged(Rect rect)
		{
			Vector2 positionMultiplier = new Vector2(0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f), 0.75f);
			scaleModel["scale"] = Math.Min(
				rect.width / (2 * (model.HalfSize.x + 1) * positionMultiplier.x),
				rect.height / (2 * (model.HalfSize.y + 1) * positionMultiplier.y));
		}

		void PopulateNodes(
            List<Model.ConstellationNode> nodeModelList_,
            List<Data.Skill.Skill> filteredSkillList_,
            Material nodeMaterial_)
		{
			foreach (var nodeModel in nodeModelList_)
			{
				NodeAdded(() =>
				{
                    if (filteredSkillList_.Contains(nodeModel.Skill))
					    return new NodeConstellation(
                            nodeModel.Skill,
					        preset,
                            hovered,
					        scaleModel,
					        nodeMaterial_,
					        nodeModel.Position);
                    else
                        return new NodeEmpty(
                            null,
                            scaleModel,
                            nodeMaterial_,
                            nodeModel.Position);
                });
			}
		}
	}
}
