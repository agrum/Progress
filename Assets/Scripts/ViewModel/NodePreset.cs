using UnityEngine;
using System.Collections.Generic;

namespace West.ViewModel
{
	public class NodePreset : INode
	{
		public event OnVoidDelegate SkillChanged = delegate { };
		public event OnBoolDelegate SelectionChanged = delegate { };
		public event OnFloatDelegate ScaleChanged = delegate { };

		private Model.Skill skill = null;
		private Model.ConstellationPreset preset;
		private Model.HoveredSkill hovered;
		private Model.Json scale;
		private Model.Skill.TypeEnum type;
		private int index;

		private bool canEdit;
		private Material mat;
		private Vector2 position;

		public NodePreset(
			Model.ConstellationPreset preset_,
			Model.HoveredSkill hovered_,
			Model.Json scale_,
			Model.Skill.TypeEnum type_,
			int index_,
			bool canEdit_, 
			Material mat_, 
			Vector2 position_)
		{
			Debug.Assert(preset_ != null);
			Debug.Assert(mat_ != null);
			
			preset = preset_;
			hovered = hovered_;
			scale = scale_;
			type = type_;
			index = index_;
			canEdit = canEdit_;
			mat = mat_;
			position = position_;

			preset.PresetUpdated += OnPresetUpdated;
			scale.ChangedEvent += OnScaleUpdated;

			OnPresetUpdated();
		}

		~NodePreset()
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
			switch (type)
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

			Model.Skill newSkill = null;
			if (list.Count > index)
				newSkill = list[index];

			if (newSkill != skill)
			{
				skill = newSkill;
				SkillChanged();
			}
		}

		public void OnScaleUpdated(string key)
		{
			ScaleChanged((float) scale["scale"].AsDouble);
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
			if (hovered != null)
				hovered.Skill = (on && skill != null) ? skill : null;
		}

		public void Clicked()
		{
			if (canEdit && skill != null)
			{
				preset.Remove(skill);
				skill = null;
				SkillChanged();
			}
		}
	}
}