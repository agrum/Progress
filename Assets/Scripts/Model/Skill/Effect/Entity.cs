using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class Entity : Base
    {
        public List<Base> Effects { get; private set; } = new List<Base>();

        public Entity(
            Id id_,
            params Base[] effects_)
            : base(id_)
        {
            Effects = new List<Base>(effects_);
        }

        public Entity(JSONNode jNode_)
            : base(jNode_)
        {
            foreach (var effect in jNode_["Effects"].AsArray)
                Effects.Add(effect.Value);
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            JSONArray effects = new JSONArray();
            foreach (var effect in Effects)
                effects.Add(effect);
            jObject["Effects"] = effects;
            return jObject;
        }

        public static Entity Projectile(
            string name_,
            SkillMetricReference radius_,
            Physics.ETravelType travelType_,
            SkillMetricReference travelParameter_,
            List<Base> hitEffects_)
        {
            return null;
        }
    }
}
