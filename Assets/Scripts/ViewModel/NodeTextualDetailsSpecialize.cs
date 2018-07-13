using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Assets.Scripts.ViewModel
{
    public class NodeTextualDetailsSpecialize : INodeTextualDetails
    {
        private Dictionary<string, Dictionary<string, string>> Map = new Dictionary<string, Dictionary<string, string>>();
        private Model.SkillSpecializer specializer;

        public NodeTextualDetailsSpecialize(Model.SkillSpecializer specializer_)
        {
            Debug.Assert(specializer_ != null);

            specializer = specializer_;
            specializer.SkillSpecialized += OnHoveredChanged;

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

        ~NodeTextualDetailsSpecialize()
        {
            specializer.SkillSpecialized -= OnHoveredChanged;
        }

        private void OnHoveredChanged()
        {
            foreach (var map in Map)
                map.Value.Clear();

            var skill = specializer.Skill();
            Model.SkillUpgrade upgrade = null;
            if (App.Content.Account.ActiveChampion != null)
                upgrade = App.Content.Account.ActiveChampion.Upgrades[skill];
                
            foreach (var metric in skill.MetrictList)
            {
                float factor = upgrade != null ? upgrade[metric].Factor() : 0;
                Map[metric.Category][metric.Name] = (metric.Value * (1 + factor)).ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            }

            Json = skill != null ? skill.Json : null;
            Emit();
        }
    }
}
