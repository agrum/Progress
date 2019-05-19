using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class Area : Base
    {
        public enum EShape
        {
            Beam,
            Cone,
        }

        public EShape Shape { get; private set; }
        public SkillMetricReference Length { get; private set; }
        public SkillMetricReference Width { get; private set; }
        public SkillMetricReference Offset { get; private set; }
        public SkillMetricReference AffectMaxCount { get; private set; }
        public List<Base> Effects { get; private set; } = new List<Base>();

        public Area(
            string name_,
            EShape shape_,
            SkillMetricReference length_,
            SkillMetricReference width_,
            SkillMetricReference offset_,
            SkillMetricReference affectMaxCount_,
            List<Base> effects_,
            ESubject from_ = ESubject.Trigger,
            ESubject to_ = ESubject.Target)
            : base(name_, from_, to_)
        {
            Shape = shape_;
            Length = length_;
            Width = width_;
            Offset = offset_;
            AffectMaxCount = affectMaxCount_;
            Effects = effects_;
        }

        public Area(JSONNode jNode_)
            : base(jNode_)
        {
            Shape = (EShape)Enum.Parse(typeof(EShape), jNode_["Shape"]);
            Length = jNode_["Length"];
            Width = jNode_["Width"];
            Offset = jNode_["Offset"];
            AffectMaxCount = jNode_["AffectMaxCount"];
            foreach (var effect in jNode_["Effects"].AsArray)
                Effects.Add(effect.Value);
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Shape"] = Shape.ToString("G");
            jObject["Length"] = Length;
            jObject["Width"] = Width;
            jObject["Offset"] = Offset;
            jObject["AffectMaxCount"] = AffectMaxCount;
            JSONArray effects = new JSONArray();
            foreach (var effect in Effects)
                effects.Add(effect);
            jObject["Effects"] = effects;
            return jObject;
        }
    }
}
