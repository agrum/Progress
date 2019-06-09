using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Layer.Medium
{
    public class Cast : Base
    {
        public MetricReference Duration { get; private set; }

        public Cast(MetricReference duration_)
        {
            Duration = duration_;
        }

        public Cast(JSONObject jObject_)
        {
            Duration = jObject_["duration"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["duration"] = Duration;
            return jObject;
        }
    }
}
