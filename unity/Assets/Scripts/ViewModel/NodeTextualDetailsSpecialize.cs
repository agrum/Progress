using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Assets.Scripts.ViewModel
{
    public class NodeTextualDetailsSpecialize : INodeTextualDetails
    {
        //private Dictionary<Data.Skill.Metric.ECategory, Dictionary<string, string>> Map = new Dictionary<Data.Skill.Metric.ECategory, Dictionary<string, string>>();
        private Model.SkillSpecializer specializer;

        public NodeTextualDetailsSpecialize(Model.SkillSpecializer specializer_)
        {
            Debug.Assert(specializer_ != null);

            specializer = specializer_;
            specializer.SkillSpecialized += OnHoveredChanged;

            //Map.Add(Data.Skill.Metric.ECategory.Misc, Misc);
            //Map.Add(Data.Skill.Metric.ECategory.Desc, Desc);
            //Map.Add(Data.Skill.Metric.ECategory.Modifier, Modifier);
            //Map.Add(Data.Skill.Metric.ECategory.Projectile, Projectile);
            //Map.Add(Data.Skill.Metric.ECategory.Charge, Charge);
            //Map.Add(Data.Skill.Metric.ECategory.Stack, Stack);
            //Map.Add(Data.Skill.Metric.ECategory.Unit, Unit);
            //Map.Add(Data.Skill.Metric.ECategory.Kit, Kit);

            OnHoveredChanged();
        }

        ~NodeTextualDetailsSpecialize()
        {
            specializer.SkillSpecialized -= OnHoveredChanged;
        }

        private void OnHoveredChanged()
        {
            //foreach (var map in Map)
            //    map.Value.Clear();

            var skill = specializer.Skill();
            Model.SkillUpgrade upgrade = null;
            if (App.Content.Account.ActiveChampion != null)
                upgrade = App.Content.Account.ActiveChampion.Upgrades[skill];
                
            //foreach (var metric in skill.Metrics)
            //{
            //    double factor = upgrade != null ? specializer.Factor(upgrade[metric]) : 0;
            //    Map[metric.Category][metric.Name.String] = (1.0 * factor).ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            //}

            //Json = skill != null ? skill : null;
            Emit();
        }
    }
}
