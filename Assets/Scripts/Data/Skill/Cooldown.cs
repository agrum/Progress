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

        public Cooldown(JSONObject jNode_)
        {
            Duration = jNode_["duration"];
        }

        public static implicit operator Cooldown(JSONNode jNode_)
        {
            if (jNode_.IsObject)
                return new Cooldown(jNode_.AsObject);
            throw new WestException("Cooldown's JSON is not an object");
        }

        public static implicit operator JSONNode(Cooldown cooldown_)
        {
            JSONObject jObject = new JSONObject();
            jObject["duration"] = cooldown_.Duration;
            return jObject;
        }
    }
}
