﻿using System;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Model.Skill
{
    public class Metric
    {
        public class UpgradeType
        {
            public enum EType
            {
                Increase,
                Decrease,
            }

            public EType Type { get; private set; }
            public int MaxUpgradeCount { get; private set; }
            public double Factor { get; private set; }

            public UpgradeType(JSONObject jNode_)
            {
                Type = (EType)Enum.Parse(typeof(EType), jNode_["Type"]);
                MaxUpgradeCount = jNode_["MaxUpgradeCount"];
                Factor = jNode_["Factor"];
            }

            public static implicit operator UpgradeType(JSONNode jNode_)
            {
                return jNode_;
            }

            public static implicit operator JSONNode(UpgradeType object_)
            {
                JSONObject jObject = new JSONObject();
                jObject["Type"] = object_.Type.ToString("G");
                jObject["MaxUpgradeCount"] = object_.MaxUpgradeCount;
                jObject["Factor"] = object_.Factor;
                return jObject;
            }
        }

        public enum EVisual
        {
            Additive,
            AdditiveMultiplier,
            Multiplier,
        }

        public NamedHash Name { get; private set; }
        public EVisual Visual { get; private set; }
        public Numeric Numeric { get; private set; }
        public UpgradeType Upgrade { get; private set; }

        public JSONObject Json { get; private set; }

        public Metric(NamedHash name_, EVisual visual_, Numeric numeric_, UpgradeType upgrade_ = null)
        {
            Name = name_;
            Visual = visual_;
            Numeric = numeric_;
            Upgrade = upgrade_;
        }

        public Metric(JSONObject jNode_)
        {
            Name = jNode_["Name"];
            Visual = (EVisual)Enum.Parse(typeof(EVisual), jNode_["Visual"]);
            Numeric = jNode_["Numeric"];
            if (!jNode_["Upgrade"].IsNull)
                Upgrade = jNode_["Upgrade"];
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
            if (numeric_.Upgrade != null)
                jObject["Upgrade"] = numeric_.Upgrade;
            return jObject;
        }
    }
}