using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill
{
    public class ModfiierBehaviour
    {
        public Trigger Trigger { get; private set; }
        public List<Condition> Conditions { get; private set; } = new List<Condition>();
        public List<Effect.Base> Effects { get; private set; } = new List<Effect.Base>();

        public static implicit operator ModfiierBehaviour(JSONObject jObject_)
        {
            ModfiierBehaviour behaviour = new ModfiierBehaviour();

            behaviour.Trigger = jObject_["Trigger"];
            foreach (var condition in jObject_["Conditions"].AsArray)
                behaviour.Conditions.Add(condition.Value.AsArray);
            foreach (var effect in jObject_["Effects"].AsArray)
                behaviour.Effects.Add(effect.Value);

            return behaviour;
        }

        public static implicit operator JSONNode(ModfiierBehaviour behaviour_)
        {
            JSONObject jObject = new JSONObject();

            jObject["Trigger"] = behaviour_.Trigger;
            var conditions = new JSONArray();
            foreach (var condition in behaviour_.Conditions)
                conditions.Add(condition);
            jObject["Conditions"] = conditions;
            var effects = new JSONArray();
            foreach (var effect in behaviour_.Effects)
                effects.Add(effect);
            jObject["Effects"] = conditions;

            return jObject;
        }
    }
}
