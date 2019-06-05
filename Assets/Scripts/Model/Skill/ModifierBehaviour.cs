using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill
{
    public class ModifierBehaviour
    {
        public Trigger.Base Trigger { get; private set; }
        public List<Condition> Conditions { get; private set; } = new List<Condition>();
        public List<Effect.Base> Effects { get; private set; } = new List<Effect.Base>();

        public ModifierBehaviour(Trigger.Base trigger_)
        {
            Trigger = trigger_;
        }

        public ModifierBehaviour(Trigger.Base trigger_, Condition[] conditions_, Effect.Base[] effects_)
        {
            Trigger = trigger_;
            Conditions = new List<Condition>(conditions_);
            Effects = new List<Effect.Base>(effects_);
        }

        public static implicit operator ModifierBehaviour(JSONNode jNode_)
        {
            ModifierBehaviour behaviour = new ModifierBehaviour(jNode_["Trigger"]);
            
            foreach (var condition in jNode_["Conditions"].AsArray)
                behaviour.Conditions.Add(condition.Value.AsArray);
            foreach (var effect in jNode_["Effects"].AsArray)
                behaviour.Effects.Add(effect.Value);

            return behaviour;
        }

        public static implicit operator JSONNode(ModifierBehaviour behaviour_)
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
