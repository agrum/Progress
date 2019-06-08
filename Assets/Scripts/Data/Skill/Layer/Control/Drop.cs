using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Layer.Control
{
    public class Drop : Base
    {
        public MetricReference Range { get; private set; }
        public MetricReference Radius { get; private set; }

        public Drop(MetricReference range_, MetricReference radius_)
        {
            Range = range_;
            Radius = radius_;
        }

        public Drop(JSONNode jNode_)
        {
            Range = jNode_["Range"];
            Radius = jNode_["Radius"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Range"] = Range;
            jObject["Radius"] = Radius;
            return jObject;
        }
    }
}
