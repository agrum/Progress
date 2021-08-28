using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.ViewModel
{
	public class NodeConstellation : INode
	{
		public event OnVoidDelegate SkillChanged = delegate { };
		public event OnBoolDelegate SelectionChanged = delegate { };
		public event OnFloatDelegate ScaleChanged = delegate { };

		private Data.Skill.Skill skill;
		private Model.ConstellationPreset preset;
		private Model.HoveredSkill hovered;
		private Model.Json scale;

		private Material mat;
		private Vector2 position;
        List<Data.Skill.Skill> selectedSkillList = null;

        public NodeConstellation(
            Data.Skill.Skill skill_,
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

            switch (skill.Category)
            {
                case Data.Skill.Skill.ECategory.Ability:
                    selectedSkillList = preset.SelectedAbilityList;
                    break;
                case Data.Skill.Skill.ECategory.Class:
                    selectedSkillList = preset.SelectedClassList;
                    break;
                case Data.Skill.Skill.ECategory.Kit:
                    selectedSkillList = preset.SelectedKitList;
                    break;
            }

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
            SelectionChanged(Selected());
		}

		public virtual void OnScaleUpdated(string key)
		{
			ScaleChanged((float)scale["scale"].AsDouble);
        }

        public bool Selected()
        {
            return selectedSkillList.Contains(skill);
        }

        public string IconPath()
		{
			return skill == null ? null : "Icons/" + skill._Id.ToString() + "/" + skill.Name;
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