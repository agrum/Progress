using UnityEngine;
using System.Collections.Generic;

namespace West.ViewModel
{
	public class NodePreset : INode
	{
		public event OnVoidChanged SkillChanged = delegate { };
		public event OnBoolChanged SelectionChanged = delegate { };
		public event OnFloatChanged ScaleChanged = delegate { };

		private Model.Skill skill;
		private Model.ConstellationPreset preset;
		private Model.HoveredSkill hovered;
		private Model.Skill.TypeEnum type;
		private int index;

		private bool canEdit;
		private Material mat;
		private Vector2 position;
		private float scale = 1.0f;

		public NodePreset(
			Model.Skill skill_,
			Model.ConstellationPreset preset_,
			Model.HoveredSkill hovered_,
			Model.Skill.TypeEnum type_,
			int index_,
			bool canEdit_, 
			Material mat_, 
			Vector2 position_)
		{
			Debug.Assert(preset_ != null);
			Debug.Assert(mat_ != null);

			skill = skill_;
			preset = preset_;
			hovered = hovered_;
			type = type_;
			index = index_;
			canEdit = canEdit_;
			mat = mat_;
			position = position_;

			preset.presetUpdateEvent += PresetUpdated;
		}

		~NodePreset()
		{
			preset.presetUpdateEvent -= PresetUpdated;

			SkillChanged = null;
			SelectionChanged = null;
			ScaleChanged = null;
		}


		public void PresetUpdated()
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

			skill = list[index];
			SkillChanged();
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

		public virtual void Scale(float scale_)
		{
			scale = scale_;
			ScaleChanged(scale);
		}

		public void Hovered(bool on)
		{
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