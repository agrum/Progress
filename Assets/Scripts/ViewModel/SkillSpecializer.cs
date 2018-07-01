using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializer
    {
        public event OnJsonDelegate SkillChanged = delegate { };

        private Model.HoveredSkill hovered = null;

        public SkillSpecializer(Model.HoveredSkill hovered_)
        {
            Debug.Assert(hovered_ != null);

            hovered = hovered_;
            hovered.ChangedEvent += OnHoveredChanged;
        }

        ~SkillSpecializer()
        {
            hovered.ChangedEvent -= OnHoveredChanged;
        }

        private void OnHoveredChanged()
        {
            if (hovered.Skill != null)
            {

            }

            SkillChanged(hovered.Skill != null ? hovered.Skill.Json : null);
        }
    }
}
