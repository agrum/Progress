using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.ViewModel
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
        List<Model.Skill> selectedSkillList = null;

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
            
            switch (type)
            {
                case Model.Skill.TypeEnum.Ability:
                    selectedSkillList = preset.SelectedAbilityList;
                    break;
                case Model.Skill.TypeEnum.Class:
                    selectedSkillList = preset.SelectedClassList;
                    break;
                case Model.Skill.TypeEnum.Kit:
                    selectedSkillList = preset.SelectedKitList;
                    break;
            }

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
			Model.Skill newSkill = null;
			if (selectedSkillList.Count > index)
				newSkill = selectedSkillList[index];
            
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

        public bool Selected()
        {
            return selectedSkillList.Contains(skill);
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

        public int Level()
        {
            if (App.Content.Account.ActiveChampion != null)
            {
                return App.Content.Account.ActiveChampion.Upgrades[skill].Level();
            }
            return 0;
        }

        public int Handicap()
        {
            if (App.Content.Account.ActiveChampion != null)
            {
                return App.Content.Account.ActiveChampion.Upgrades[skill].Handicap();
            }
            return 0;
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
			}
		}
	}
}