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
		public class Constellation
		{
			public View.NodeTextualDetails nodeTextualDetails = null;

			private View.Constellation view = null;
			private Model.Constellation model = null;
			private Model.ConstellationPreset preset = null;
			private Model.HoveredSkill hoveredModel = null;
			private Model.Json scaleModel = new Model.Json();

			private RectTransform canvas = null;
			private GameObject prefab = null;
			private Material abilityMaterial = null;
			private Material classMaterial = null;
			private Material kitMaterial = null;

			public Constellation(View.Constellation view_, Model.Constellation model_, Model.ConstellationPreset preset_)
			{
				view = view_;
				model = model_;
				preset = preset_;
				scaleModel["scale"] = 1.0;
				canvas = view.GetComponent<RectTransform>();
				prefab = App.Resource.Prefab.ConstellationNode;
				abilityMaterial = App.Resource.Material.AbilityMaterial;
				classMaterial = App.Resource.Material.ClassMaterial;
				kitMaterial = App.Resource.Material.KitMaterial;

				view.SizeChangedEvent += OnSizeChanged;

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


			void OnSizeChanged()
			{
				Vector2 positionMultiplier = new Vector2(0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f), 0.75f);
				scaleModel["scale"] = Math.Min(
					canvas.rect.width / (2 * (model.HalfSize.x + 1) * positionMultiplier.x),
					canvas.rect.height / (2 * (model.HalfSize.y + 1) * positionMultiplier.y));
			}

			void PopulateNodes(List<Model.ConstellationNode> nodeModelList_, Material nodeMaterial_)
			{
				foreach (var nodeModel in nodeModelList_)
				{
					GameObject gob = GameObject.Instantiate(prefab);
					gob.transform.SetParent(canvas.transform);

					gob.GetComponent<View.Node>().SetContext(new NodeConstellation(
						nodeModel.Skill,
						preset,
						hoveredModel,
						scaleModel,
						nodeMaterial_,
						nodeModel.Position));
				}
			}
		}
	}
}
