using UnityEngine;
using System.Collections.Generic;
using System;

namespace West.ViewModel
{
	public class NodeConstellation : INode
	{
		public event OnVoidDelegate SkillChanged = delegate { };
		public event OnBoolDelegate SelectionChanged = delegate { };
		public event OnFloatDelegate ScaleChanged = delegate { };

		private Model.Skill skill;
		private Model.ConstellationPreset preset;
		private Model.HoveredSkill hovered;
		private Model.Json scale;

		private Material mat;
		private Vector2 position;

		public NodeConstellation(
			Model.Skill skill_,
			Model.ConstellationPreset preset_,
			Model.HoveredSkill hovered_,
			Model.Json scale_,
			Material mat_,
			Vector2 position_)
		{
			Debug.Assert(skill_ != null);
			Debug.Assert(preset_ != null);
			Debug.Assert(hovered_ != null);
			Debug.Assert(mat_ != null);

			skill = skill_;
			preset = preset_;
			hovered = hovered_;
			scale = scale_;
			mat = mat_;
			position = position_;
			
			preset.PresetUpdated += OnPresetUpdated;
			scale.ChangedEvent += OnScaleUpdated;
		}

		~NodeConstellation()
		{
			preset.PresetUpdated -= OnPresetUpdated;
			scale.ChangedEvent -= OnScaleUpdated;

			SkillChanged = null;
			SelectionChanged = null;
			ScaleChanged = null;
		}

		public void OnPresetUpdated()
		{
			List<Model.Skill> list = null;
			switch (skill.Type)
			{
				case Model.Skill.TypeEnum.Ability:
					list = preset.SelectedAbilityList;
					break;
				case Model.Skill.TypeEnum.Class:
					list = preset.SelectedClassList;
					break;
				case Model.Skill.TypeEnum.Kit:
					list = preset.SelectedKitList;
					break;
			}

			SelectionChanged(list.Contains(skill));
		}

		public virtual void OnScaleUpdated(string key)
		{
			ScaleChanged((float)scale["scale"].AsDouble);
		}

		public string IconPath()
		{
			return skill == null ? null : "Icons/" + skill.UpperCamelCaseKey + "/" + skill.Json["name"];
		}

		public Material Mat()
		{
			return mat;
		}

		public Vector2 Position()
		{
			return position;
		}

		public void Hovered(bool on)
		{
			hovered.Skill = (on && skill != null) ? skill : null;
		}

		public void Clicked()
		{
			try
			{
				if (preset.Has(skill))
					preset.Remove(skill);
				else
					preset.Add(skill);
			}
			catch (Exception)
			{
				Debug.Log("ViewModel.NodeConstellation.Clicked()");
			}
		}
	}
}