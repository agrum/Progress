using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Input
{
    public abstract class Base
    {
        public Activation.Base Activation { get; private set; }
        public Control.Base Control { get; private set; }
        public Condition Condition { get; private set; }
        public Medium.Base Medium { get; private set; }
        public List<Effect.Base> Effects { get; private set; } = new List<Effect.Base>();

        public Base(
            Activation.Base activation_,
            Control.Base control_,
            Condition condition_,
            Medium.Base medium_,
            Effect.Base[] effects_)
        {
            Activation = activation_;
            Control = control_;
            Condition = condition_;
            Medium = medium_;
            Effects = new List<Effect.Base>(effects_);
        }

        public Base(JSONNode jNode_)
        {
            Activation = jNode_["Activation"];
            Control = jNode_["Control"];
            Condition = jNode_["Condition"];
            Medium = jNode_["Medium"];
            foreach (var effect in jNode_["Effects"].AsArray)
                Effects.Add(effect.Value);
        }

        public static implicit operator Base(JSONNode jNode)
        {
            return jNode;
        }

        public static implicit operator JSONNode(Base object_)
        {
            JSONObject jObject = new JSONObject();
            jObject["Activation"] = object_.Activation;
            jObject["Control"] = object_.Control;
            jObject["Condition"] = object_.Condition;
            jObject["Medium"] = object_.Medium;
            var effects = new JSONArray();
            foreach (var effect in object_.Effects)
                effects.AsArray.Add(effect);
            jObject["Effects"] = effects;
            return jObject;
        }
    }
}
