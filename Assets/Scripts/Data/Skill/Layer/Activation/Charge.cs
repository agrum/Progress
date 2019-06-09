using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Layer.Activation
{
    class Charge : Base
    {
        public MetricReference MaxChargeDuration { get; private set; }
        public MetricReference MaxHoldDuration { get; private set; }

        public Charge(
            MetricReference maxChargeDuration_,
            MetricReference maxHoldDuration_)
        {
            MaxChargeDuration = maxChargeDuration_;
            MaxHoldDuration = maxHoldDuration_;
        }

        public Charge(JSONNode jNode_)
        {
            MaxChargeDuration = jNode_["maxChargeDuration"];
            MaxHoldDuration = jNode_["maxHoldDuration"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["maxHoldDuration"] = MaxChargeDuration;
            jObject["maxChargeDuration"] = MaxHoldDuration;
            return jObject;
        }
    }
}
