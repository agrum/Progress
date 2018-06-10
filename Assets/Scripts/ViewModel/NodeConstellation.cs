using UnityEngine;
using System.Collections.Generic;
using System;

namespace West
{
	namespace ViewModel
	{
		public class NodeConstellation
		{
			private View.Node viewNode;
			private View.NodeTextualDetails viewDetails;
			private Model.ConstellationNode modelNode;
			private Model.ConstellationPreset modelPreset;
			private Material mat;
			private Vector2 position;
			private float scale = 1.0f;

			public NodeConstellation(
				View.Node viewNode_,
				View.NodeTextualDetails viewDetails_,
				Model.ConstellationNode modelNode_,
				Model.ConstellationPreset modelPreset_,
				Material mat_,
				Vector2 position_)
			{
				Debug.Assert(viewNode != null);
				Debug.Assert(modelNode_ != null);
				Debug.Assert(modelPreset != null);
				Debug.Assert(mat != null);

				viewNode = viewNode_;
				viewDetails = viewDetails_;
				modelNode = modelNode_;
				modelPreset = modelPreset_;
				mat = mat_;
				position = position_;

				viewNode.hoveredEvent += Hovered;
				viewNode.clickedEvent += Clicked;
				modelPreset.presetUpdateEvent += PresetUpdated;

				viewNode.Setup(
					modelNode == null ? null : "Icons/" + modelNode.Skill.UpperCamelCaseKey + "/" + modelNode.Skill.Json["name"],
					mat,
					position);
				PresetUpdated();
			}

			~NodeConstellation()
			{
				viewNode.hoveredEvent -= Hovered;
				viewNode.clickedEvent -= Clicked;
			}

			public void PresetUpdated()
			{
				List<Model.Skill> list = null;
				switch (modelNode.Skill.Type)
				{
					case Model.Skill.TypeEnum.Ability:
						list = modelPreset.SelectedAbilityList;
						break;
					case Model.Skill.TypeEnum.Class:
						list = modelPreset.SelectedClassList;
						break;
					case Model.Skill.TypeEnum.Kit:
						list = modelPreset.SelectedKitList;
						break;
				}

				viewNode.Selected = list.Contains(modelNode.Skill);
			}

			public virtual void Scale(float scale_)
			{
				scale = scale_;
				viewNode.Scale(scale);
			}

			public void Hovered(bool on)
			{
				if (viewDetails != null)
					viewDetails.Setup((on && modelNode != null) ? modelNode.Skill : null, mat);
			}

			public void Clicked()
			{
				try
				{
					if (modelPreset.Has(modelNode.Skill))
						modelPreset.Remove(modelNode.Skill);
					else
						modelPreset.Add(modelNode.Skill);
				}
				catch (Exception)
				{
					Debug.Log("ViewModel.NodeConstellation.Clicked()");
				}
			}
		}
	}
}