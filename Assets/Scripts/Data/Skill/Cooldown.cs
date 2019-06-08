using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill
{
    public class Cooldown
    {
        public NamedHash Type { get; private set; }
        public MetricReference Duration { get; private set; }

        public Cooldown(NamedHash type_)
        {
            Type = type_;
        }

        public Cooldown(JSONNode jNode_)
        {
            Duration = jNode_["Duration"];
        }

        public static implicit operator Cooldown(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator JSONNode(Cooldown cooldown_)
        {
            JSONObject jObject = new JSONObject();
            jObject["Duration"] = cooldown_.Duration;
            return jObject;
        }
    }
}
