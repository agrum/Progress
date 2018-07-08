using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Assets.Scripts.ViewModel
{
    public class NodeTextualDetails
    {
        public event OnVoidDelegate SkillChanged = delegate { };

        public Dictionary<string, string> Misc = new Dictionary<string, string>();
        public Dictionary<string, string> Desc = new Dictionary<string, string>();
        public Dictionary<string, string> Modifier = new Dictionary<string, string>();
        public Dictionary<string, string> Projectile = new Dictionary<string, string>();
        public Dictionary<string, string> Charge = new Dictionary<string, string>();
        public Dictionary<string, string> Stack = new Dictionary<string, string>();
        public Dictionary<string, string> Unit = new Dictionary<string, string>();
        public Dictionary<string, string> Kit = new Dictionary<string, string>();

        public JSONNode Skill { get; private set; } = null;

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

        private void OnHoveredChanged()
        {
            foreach (var map in Map)
                map.Value.Clear();

            if (hovered.Skill != null)
            {
                JSONArray metrics = hovered.Skill.Json["metrics2"].AsArray;
                foreach (var almostMetric in metrics)
                {
                    JSONObject metric = almostMetric.Value.AsObject;
                    string category = metric["category"];
                    string name = metric["name"];
                    var merp = Map[metric["category"]];
                    Map[metric["category"]][metric["name"]] = metric["value"]; 
                }
            }

            Skill = hovered.Skill != null ? hovered.Skill.Json : null;
            SkillChanged();
        }
    }
}
