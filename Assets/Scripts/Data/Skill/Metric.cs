using System;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Skill
{
    public class Metric
    {
        public enum ECategory
        {
            Misc,
            Desc,
            Modifier,
            Projectile,
            Charge,
            Stack,
            Unit,
            Kit,
        }

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
                Sign = Serializer.ReadEnum<ESign>(jNode_["sign"]);
                MaxUpgradeCount = jNode_["maxUpgradeCount"];
                Factor = jNode_["factor"];
            }

            public static implicit operator UpgradeType(JSONNode jNode_)
            {
                if (jNode_.IsObject)
                    return new UpgradeType(jNode_.AsObject);
                throw new WestException("UpgradeType's JSON is not an object");
            }

            public static implicit operator JSONNode(UpgradeType object_)
            {
                JSONObject jObject = new JSONObject();
                jObject["sign"] = Serializer.WriteEnum(object_.Sign);
                jObject["maxUpgradeCount"] = object_.MaxUpgradeCount;
                jObject["factor"] = object_.Factor;
                return jObject;
            }
        }

        public string _Id { get; private set; }
        public NamedHash Name { get; private set; }
        public ECategory Category { get; private set; }
        public Numeric Numeric { get; private set; }
        public UpgradeType Upgrade { get; private set; }

        public Metric(NamedHash name_, ECategory category_, Numeric numeric_, UpgradeType upgrade_ = null)
        {
            _Id = Guid.NewGuid().ToString();
            Name = name_;
            Category = category_;
            Numeric = numeric_;
            Upgrade = upgrade_;
        }

        public Metric(JSONObject jObject_)
        {
            _Id = jObject_["_id"];
            Name = jObject_["name"];
            Category = Serializer.ReadEnum<ECategory>(jObject_["category"]);
            Numeric = jObject_["numeric"];
            if (!jObject_["upgrade"].IsNull)
                Upgrade = jObject_["upgrade"];
        }

        public static implicit operator Metric(JSONNode jNode_)
        {
            if (jNode_.IsObject)
                return new Metric(jNode_.AsObject);
            throw new WestException("Metric's JSON is not an object");
        }

        public static implicit operator JSONNode(Metric object_)
        {
            JSONObject jObject = new JSONObject();
            jObject["_id"] = object_._Id.ToString();
            jObject["name"] = object_.Name;
            jObject["category"] = Serializer.WriteEnum(object_.Category);
            jObject["numeric"] = object_.Numeric;
            if (object_.Upgrade != null)
                jObject["upgrade"] = object_.Upgrade;
            return jObject;
        }
    }
}
