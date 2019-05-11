using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill
{
    public class ModfiierEffect
    {
        public Trigger Type { get; private set; }
        public List<Condition> Conditions { get; private set; } = new List<Condition>();

        public static implicit operator ModfiierEffect(JSONObject jObject_)
        {
            ModfiierEffect trigger = new ModfiierEffect();

            trigger.Type = jObject_["type"];
            foreach (var node in jObject_["conditions"].AsArray)
            {
                trigger.Conditions.Add(node.Value.AsArray);
            }

            return trigger;
        }

        public static implicit operator JSONNode(ModfiierEffect trigger_)
        {
            JSONObject jObject = new JSONObject();

            jObject["type"] = trigger_.Type;
            var conditions = new JSONArray();
            foreach (var condition in trigger_.Conditions)
                conditions.Add(condition);
            jObject["conditions"] = conditions;

            return jObject;
        }
    }
}
