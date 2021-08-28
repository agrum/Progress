using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Layer.Control
{
    public class Beam : Base
    {
        public MetricReference Length { get; private set; }
        public MetricReference Width { get; private set; }

        public Beam(MetricReference length_, MetricReference width_)
        {
            Length = length_;
            Width = width_;
        }

        public Beam(JSONNode jNode_)
        {
            Length = jNode_["length"];
            Width = jNode_["width"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["length"] = Length;
            jObject["width"] = Width;
            return jObject;
        }
    }
}
