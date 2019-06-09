using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
{
    public class Unit : Base
    {
        public List<Base> Effects { get; private set; } = new List<Base>();

        public Unit(
            Id id_,
            params Base[] effects_)
            : base(id_)
        {
            Effects = new List<Base>(effects_);
        }

        public Unit(JSONNode jNode_)
            : base(jNode_)
        {
            foreach (var effect in jNode_["effects"].AsArray)
                Effects.Add(effect.Value);
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            JSONArray effects = new JSONArray();
            foreach (var effect in Effects)
                effects.Add(effect);
            jObject["effects"] = effects;
            return jObject;
        }
    }
}
