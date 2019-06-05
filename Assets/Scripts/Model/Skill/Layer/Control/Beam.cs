using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Layer.Control
{
    public class Beam : Base
    {
        public SkillMetricReference Length { get; private set; }
        public SkillMetricReference Width { get; private set; }

        public Beam(SkillMetricReference length_, SkillMetricReference width_)
        {
            Length = length_;
            Width = width_;
        }

        public Beam(JSONNode jNode_)
        {
            Length = jNode_["Length"];
            Width = jNode_["Width"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Length"] = Length;
            jObject["Width"] = Width;
            return jObject;
        }
    }
}
