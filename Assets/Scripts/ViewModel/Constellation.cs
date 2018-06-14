using System;
using System.Collections.Generic;
using UnityEngine;

namespace West.ViewModel
{
	public class Constellation : INodeMap
	{
		public event OnElementAdded NodeAdded = delegate { };

		private Model.Constellation model = null;
		private Model.ConstellationPreset preset = null;
		private Model.HoveredSkill hoveredModel = null;
		private Model.Json scaleModel = new Model.Json();
		
		private Material abilityMaterial = null;
		private Material classMaterial = null;
		private Material kitMaterial = null;

		public Constellation(Model.Constellation model_, Model.ConstellationPreset preset_)
		{
			model = model_;
			preset = preset_;
			scaleModel["scale"] = 1.0;
			abilityMaterial = App.Resource.Material.AbilityMaterial;
			classMaterial = App.Resource.Material.ClassMaterial;
			kitMaterial = App.Resource.Material.KitMaterial;

			//create constellation nodes
			PopulateNodes(
				model.AbilityNodeList,
				abilityMaterial);
			PopulateNodes(
				model.ClassNodeList,
				classMaterial);
			PopulateNodes(
				model.KitNodeList,
				kitMaterial);
		}

		public void SizeChanged(Rect rect)
		{
			Vector2 positionMultiplier = new Vector2(0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f), 0.75f);
			scaleModel["scale"] = Math.Min(
				rect.width / (2 * (model.HalfSize.x + 1) * positionMultiplier.x),
				rect.height / (2 * (model.HalfSize.y + 1) * positionMultiplier.y));
		}

		void PopulateNodes(List<Model.ConstellationNode> nodeModelList_, Material nodeMaterial_)
		{
			foreach (var nodeModel in nodeModelList_)
			{
				NodeAdded(() =>
				{
					return new NodeConstellation(
					nodeModel.Skill,
					preset,
					hoveredModel,
					scaleModel,
					nodeMaterial_,
					nodeModel.Position);
				});
			}
		}
	}
}
