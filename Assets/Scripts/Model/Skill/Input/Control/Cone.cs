using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Input.Control
{
    public class Cone : Base
    {
        public SkillMetricReference Radius { get; private set; }
        public SkillMetricReference Degrees { get; private set; }

        public Cone(SkillMetricReference radius_, SkillMetricReference degrees_)
        {
            Radius = length_;
            Degrees = degrees_;
        }

        public Cone(JSONNode jNode_)
        {
            Radius = jNode_["Radius"];
            Degrees = jNode_["Degrees"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Radius"] = Radius;
            jObject["Degrees"] = Degrees;
            return jObject;
        }
    }
}
