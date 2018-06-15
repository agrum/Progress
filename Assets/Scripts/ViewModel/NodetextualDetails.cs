using System;
using System.Collections.Generic;
using UnityEngine;

namespace West.ViewModel
{
    public class NodeTextualDetails
    {
        public event OnJsonDelegate SkillChanged = delegate { };

        private Model.HoveredSkill hovered;

        public NodeTextualDetails(Model.HoveredSkill hovered_)
        {
            hovered = hovered_;
            hovered.ChangedEvent += OnHoveredChanged;
        }

        private void OnHoveredChanged()
        {
            SkillChanged(hovered.Skill != null ? hovered.Skill.Json : null);
        }
    }
}
