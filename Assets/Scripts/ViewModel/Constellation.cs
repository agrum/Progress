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
			private RectTransform canvas = null;
			private GameObject prefab = null;
			private Material abilityMaterial = null;
			private Material classMaterial = null;
			private Material kitMaterial = null;

			private List<NodeConstellation> abilityNodeList = new List<NodeConstellation>();
			private List<NodeConstellation> classNodeList = new List<NodeConstellation>();
			private List<NodeConstellation> kitNodeList = new List<NodeConstellation>();

			public void Setup(Model.Constellation model_, Model.ConstellationPreset preset_)
			{
				model = model_;
				preset = preset_;
				canvas = view.GetComponent<RectTransform>();
				prefab = App.Resource.Prefab.ConstellationNode;
				abilityMaterial = App.Resource.Material.AbilityMaterial;
				classMaterial = App.Resource.Material.ClassMaterial;
				kitMaterial = App.Resource.Material.KitMaterial;

				view.SizeChangedEvent += OnSizeChanged;

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
			}


			void OnSizeChanged()
			{
				Vector2 positionMultiplier = new Vector2(0.5f * (float)Math.Cos(30.0f * Math.PI / 180.0f), 0.75f);
				float scale = Math.Min(
					canvas.rect.width / (2 * (model.HalfSize.x + 1) * positionMultiplier.x),
					canvas.rect.height / (2 * (model.HalfSize.y + 1) * positionMultiplier.y));

				foreach (var node in abilityNodeList)
					node.Scale(scale);
				foreach (var node in classNodeList)
					node.Scale(scale);
				foreach (var node in kitNodeList)
					node.Scale(scale);
			}

			void PopulateNodes(List<Model.ConstellationNode> nodeModelList_, Material nodeMaterial_, ref List<ViewModel.NodeConstellation> nodeList_)
			{
				foreach (var nodeModel in nodeModelList_)
				{
					GameObject gob = GameObject.Instantiate(prefab);
					gob.transform.SetParent(canvas.transform);

					NodeConstellation node = new NodeConstellation(
						gob.AddComponent<View.Node>(),
						nodeTextualDetails,
						nodeModel,
						preset,
						nodeMaterial_,
						nodeModel.Position);
					nodeList_.Add(node);
				}
			}
		}
	}
}
