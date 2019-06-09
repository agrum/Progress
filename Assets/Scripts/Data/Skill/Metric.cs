using System;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Skill
{
    public class Metric
    {
        public class UpgradeType
        {
            public enum ESign
            {
                Positive,
                Negative,
            }

            public ESign Sign { get; private set; }
            public int MaxUpgradeCount { get; private set; }
            public double Factor { get; private set; }

            public UpgradeType(ESign sign_, int maxUpgradeCount_, double factor_)
            {
                Sign = sign_;
                MaxUpgradeCount = maxUpgradeCount_;
                Factor = factor_;
            }

            public UpgradeType(JSONObject jNode_)
            {
                Sign = Serializer.ReadEnum<ESign>(jNode_["Sign"]);
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
                jObject["Sign"] = Serializer.WriteEnum(object_.Sign);
                jObject["MaxUpgradeCount"] = object_.MaxUpgradeCount;
                jObject["Factor"] = object_.Factor;
                return jObject;
            }
        }

        public Guid _Id { get; private set; }
        public NamedHash Name { get; private set; }
        public Numeric Numeric { get; private set; }
        public UpgradeType Upgrade { get; private set; }

        public Metric(NamedHash name_, Numeric numeric_, UpgradeType upgrade_ = null)
        {
            _Id = Guid.NewGuid();
            Name = name_;
            Numeric = numeric_;
            Upgrade = upgrade_;
        }

        public Metric(JSONObject jNode_)
        {
            _Id = new Guid(jNode_["_id"]);
            Name = jNode_["Name"];
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
            jObject["_id"] = numeric_._Id.ToString();
            jObject["Name"] = numeric_.Name;
            jObject["Numeric"] = numeric_.Numeric;
            if (numeric_.Upgrade != null)
                jObject["Upgrade"] = numeric_.Upgrade;
            return jObject;
        }
    }
}
