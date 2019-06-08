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
