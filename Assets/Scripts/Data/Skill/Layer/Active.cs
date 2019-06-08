using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Layer
{
    public class Active : Base
    {
        public Activation.Base Activation { get; private set; }
        public Control.Base Control { get; private set; }
        public Condition Condition { get; private set; }
        public Medium.Base Medium { get; private set; }
        public List<Effect.Base> Effects { get; private set; } = new List<Effect.Base>();

        public Active(
            Visual visual_,
            Activation.Base activation_,
            Control.Base control_,
            Condition condition_,
            Medium.Base medium_,
            Effect.Base[] effects_)
            : base(visual_)
        {
            Activation = activation_;
            Control = control_;
            Condition = condition_;
            Medium = medium_;
            Effects = new List<Effect.Base>(effects_);
        }

        public Active(JSONNode jNode_)
            : base(jNode_)
        {
            Activation = jNode_["Activation"];
            Control = jNode_["Control"];
            Condition = jNode_["Condition"];
            Medium = jNode_["Medium"];
            foreach (var effect in jNode_["Effects"].AsArray)
                Effects.Add(effect.Value);
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Activation"] = Activation;
            jObject["Control"] = Control;
            jObject["Condition"] = Condition;
            jObject["Medium"] = Medium;
            var effects = new JSONArray();
            foreach (var effect in Effects)
                effects.AsArray.Add(effect);
            jObject["Effects"] = effects;
            return jObject;
        }
    }
}
