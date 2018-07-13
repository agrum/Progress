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
        private Dictionary<string, Dictionary<string, string>> Map = new Dictionary<string, Dictionary<string, string>>();
        private Model.HoveredSkill hovered;

        public NodeTextualDetails(Model.HoveredSkill hovered_)
        {
            hovered = hovered_;
            hovered.ChangedEvent += OnHoveredChanged;

            Map.Add("misc", Misc);
            Map.Add("desc", Desc);
            Map.Add("modifier", Modifier);
            Map.Add("projectile", Projectile);
            Map.Add("charge", Charge);
            Map.Add("stack", Stack);
            Map.Add("unit", Unit);
            Map.Add("kit", Kit);

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
                
                foreach (var metric in hovered.Skill.MetrictList)
                {
                    float factor = upgrade != null ? upgrade[metric].Factor() : 0;
                    Map[metric.Category][metric.Name] = (metric.Value * (1 + factor)).ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            Json = hovered.Skill != null ? hovered.Skill.Json : null;
            Emit();
        }
    }
}
