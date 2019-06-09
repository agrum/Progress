using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Layer.Control
{
    public class None : Base
    {
        public MetricReference Radius { get; private set; }

        public None(MetricReference radius_)
        {
            Radius = radius_;
        }

        public None(JSONNode jNode_)
        {
            Radius = jNode_["radius"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["radius"] = Radius;
            return jObject;
        }
    }
}
