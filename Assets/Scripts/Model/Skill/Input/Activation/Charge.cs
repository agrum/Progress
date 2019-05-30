using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Input.Activation
{
    class Charge : Base
    {
        public SkillMetricReference MaxChargeDuration { get; private set; }
        public SkillMetricReference MaxHoldDuration { get; private set; }

        public Charge(
            SkillMetricReference maxChargeDuration_,
            SkillMetricReference maxHoldDuration_)
        {
            MaxChargeDuration = maxChargeDuration_;
            MaxHoldDuration = maxHoldDuration_;
        }

        public Charge(JSONNode jNode_)
        {
            MaxChargeDuration = jNode_["MaxChargeDuration"];
            MaxHoldDuration = jNode_["MaxHoldDuration"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["MaxHoldDuration"] = MaxChargeDuration;
            jObject["MaxChargeDuration"] = MaxHoldDuration;
            return jObject;
        }
    }
}
