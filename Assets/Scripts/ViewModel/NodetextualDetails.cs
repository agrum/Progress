using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Assets.Scripts.ViewModel
{
    public class NodeTextualDetails
    {
        public event OnJsonDelegate SkillChanged = delegate { };

        public Dictionary<string, string> Desc { get; private set; } = null;
        public Dictionary<string, string> Misc { get; private set; } = null;
        public Dictionary<string, string> Modifler { get; private set; } = null;
        private Model.HoveredSkill hovered;

        public NodeTextualDetails(Model.HoveredSkill hovered_)
        {
            hovered = hovered_;
            hovered.ChangedEvent += OnHoveredChanged;
        }

        ~NodeTextualDetails()
        {
            hovered.ChangedEvent -= OnHoveredChanged;
        }

        private void OnHoveredChanged()
        {
            Modifler = null;

            if (hovered.Skill != null)
            {
                JSONArray metrics = hovered.Skill.Json["metrics2"].AsArray;
                foreach (var almostMetric in metrics)
                {
                    JSONObject metric = almostMetric.Value.AsObject;
                    if (metric["category"] == "desc")
                    {
                        if (Desc == null)
                            Desc = new Dictionary<string, string>();
                        Desc[metric["name"]] = metric["value"];
                    }
                    else if (metric["category"] == "misc")
                    {
                        if (Misc == null)
                            Misc = new Dictionary<string, string>();
                        Misc[metric["name"]] = metric["value"];
                    }
                    else if (metric["category"] == "modifier")
                    {
                        if (Modifler == null)
                            Modifler = new Dictionary<string, string>();
                        Modifler[metric["name"]] = metric["value"];
                    }
                }
            }

            SkillChanged(hovered.Skill != null ? hovered.Skill.Json : null);
        }
    }
}
