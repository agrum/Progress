using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Layer.Control
{
    public class None : Base
    {
        public SkillMetricReference Radius { get; private set; }

        public None(SkillMetricReference radius_)
        {
            Radius = radius_;
        }

        public None(JSONNode jNode_)
        {
            Radius = jNode_["Radius"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Radius"] = Radius;
            return jObject;
        }
    }
}
