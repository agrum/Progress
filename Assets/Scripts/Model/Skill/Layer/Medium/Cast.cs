using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Layer.Medium
{
    public class Cast : Base
    {
        public SkillMetricReference Duration { get; private set; }

        public Cast(SkillMetricReference duration_)
        {
            Duration = duration_;
        }

        public Cast(JSONNode jNode_)
        {
            Duration = jNode_["Duration"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Duration"] = Duration;
            return jObject;
        }
    }
}
