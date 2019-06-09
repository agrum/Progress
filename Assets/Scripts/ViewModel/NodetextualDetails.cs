using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Assets.Scripts.ViewModel
{
    public abstract class INodeTextualDetails
    {
        public event OnVoidDelegate SkillChanged = delegate { };

        public JSONNode Json { get; protected set; } = null;

        public Dictionary<string, string> Misc = new Dictionary<string, string>();
        public Dictionary<string, string> Desc = new Dictionary<string, string>();
        public Dictionary<string, string> Modifier = new Dictionary<string, string>();
        public Dictionary<string, string> Projectile = new Dictionary<string, string>();
        public Dictionary<string, string> Charge = new Dictionary<string, string>();
        public Dictionary<string, string> Stack = new Dictionary<string, string>();
        public Dictionary<string, string> Unit = new Dictionary<string, string>();
        public Dictionary<string, string> Kit = new Dictionary<string, string>();

        protected void Emit()
        {
            SkillChanged();
        }

        ~INodeTextualDetails()
        {
            SkillChanged = null;
        }
    }

    public class NodeTextualDetails : INodeTextualDetails
    {
        private Dictionary<Data.Skill.Metric.ECategory, Dictionary<string, string>> Map = new Dictionary<Data.Skill.Metric.ECategory, Dictionary<string, string>>();
        private Model.HoveredSkill hovered;

        public NodeTextualDetails(Model.HoveredSkill hovered_)
        {
            hovered = hovered_;
            hovered.ChangedEvent += OnHoveredChanged;

            Map.Add(Data.Skill.Metric.ECategory.Misc, Misc);
            Map.Add(Data.Skill.Metric.ECategory.Desc, Desc);
            Map.Add(Data.Skill.Metric.ECategory.Modifier, Modifier);
            Map.Add(Data.Skill.Metric.ECategory.Projectile, Projectile);
            Map.Add(Data.Skill.Metric.ECategory.Charge, Charge);
            Map.Add(Data.Skill.Metric.ECategory.Stack, Stack);
            Map.Add(Data.Skill.Metric.ECategory.Unit, Unit);
            Map.Add(Data.Skill.Metric.ECategory.Kit, Kit);

            OnHoveredChanged();
        }

        ~NodeTextualDetails()
        {
            hovered.ChangedEvent -= OnHoveredChanged;
        }

        protected void OnHoveredChanged()
        {
            foreach (var map in Map)
                map.Value.Clear();

            if (hovered.Skill != null)
            {
                Model.SkillUpgrade upgrade = null;
                if (App.Content.Account.ActiveChampion != null)
                    upgrade = App.Content.Account.ActiveChampion.Upgrades[hovered.Skill];
                
                foreach (var metric in hovered.Skill.Metrics)
                {
                    double factor = upgrade != null ? upgrade[metric].Factor() : 0;
                    Map[metric.Category][metric.Name.String] = (1.0 * factor).ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            Json = hovered?.Skill ?? null;
            Emit();
        }
    }
}
