using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Layer.Control
{
    public class Cone : Base
    {
        public MetricReference Radius { get; private set; }
        public MetricReference Degrees { get; private set; }

        public Cone(MetricReference radius_, MetricReference degrees_)
        {
            Radius = radius_;
            Degrees = degrees_;
        }

        public Cone(JSONNode jNode_)
        {
            Radius = jNode_["radius"];
            Degrees = jNode_["degrees"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["radius"] = Radius;
            jObject["degrees"] = Degrees;
            return jObject;
        }
    }
}
