using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill
{
    public class EffectUnitAttribute
    {
        public enum ECategory : int
        {
            Misc,
            Desc,
            Projectile,
            Unit
        }

        public enum EIntegration : int
        {
            Additive,
            AdditiveMultiplier,
            Multiplier
        }

        public enum EUpgradeType : int
        {
            None,
            Incremental,
            Reduction
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public ECategory Category { get; private set; }
        public Numeric Numeric { get; private set; }
        public EIntegration Integration { get; private set; }
        public EUpgradeType UpgType { get; private set; }
        public float UpgCost { get; private set; }

        EffectUnitAttribute(
            string id_, 
            string name_, 
            ECategory category_, 
            Numeric numeric_, 
            EIntegration integration_,
            EUpgradeType upgType_, 
            float upgCost_)
        {
            Id = id_;
            Name = name_;
            Category = category_;
            Numeric = numeric_;
            Integration = integration_;
            UpgType = upgType_;
            UpgCost = upgCost_;
        }

        EffectUnitAttribute(
            JSONObject jObject_)
        {
            Id = jObject_["nameId"];
            Name = jObject_["name"];
            Category = (ECategory) jObject_["category"].AsInt;
            Numeric = jObject_["numeric"];
            Integration = (EIntegration) jObject_["integration"].AsInt;
            UpgType = (EUpgradeType) jObject_["upgType"].AsInt;
            UpgCost = jObject_["upgCost"];
        }

        public static implicit operator EffectUnitAttribute(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator JSONNode(EffectUnitAttribute skillEffectUnitAttribute_)
        {
            JSONObject jObject = new JSONObject();
            jObject["nameId"] = skillEffectUnitAttribute_.Id;
            jObject["name"] = skillEffectUnitAttribute_.Name;
            jObject["category"] = (int)skillEffectUnitAttribute_.Category;
            jObject["numeric"] = skillEffectUnitAttribute_.Numeric;
            jObject["integration"] = (int)skillEffectUnitAttribute_.Integration;
            jObject["upgType"] = (int)skillEffectUnitAttribute_.UpgType;
            jObject["upgCost"] = skillEffectUnitAttribute_.UpgCost;
            return jObject;
        }
    }
}
