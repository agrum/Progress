using System;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Model.Skill
{
    public class Metric
    {
        public enum EVisual
        {
            Additive,
            AdditiveMultiplier,
            Multiplier,
        }

        public NamedHash Name { get; private set; }
        public EVisual Visual { get; private set; }
        public Numeric Numeric { get; private set; }
        public int MaxUpgrade { get; private set; }

        public JSONObject Json { get; private set; }

        public Metric(JSONObject jNode_)
        {
            Name = jNode_["Name"];
            Visual = (EVisual)Enum.Parse(typeof(EVisual), jNode_["EVisual"]);
            Numeric = jNode_["Numeric"];
            MaxUpgrade = jNode_["MaxUpgrade"];
        }

        public static implicit operator Metric(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator JSONNode(Metric numeric_)
        {
            JSONObject jObject = new JSONObject();
            jObject["Name"] = numeric_.Name;
            jObject["Visual"] = numeric_.Visual.ToString("G");
            jObject["Numeric"] = numeric_.Numeric;
            jObject["MaxUpgrade"] = numeric_.MaxUpgrade;
            return jObject;
        }
    }
}
