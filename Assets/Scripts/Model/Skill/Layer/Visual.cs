using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Layer
{
    public class Visual
    {
        public MetricReference Middle { get; private set; }
        public MetricReference TopRight { get; private set; }

        public Visual(MetricReference middle_)
        {
            Middle = middle_;
        }

        public Visual(MetricReference middle_, MetricReference topRight_)
        {
            Middle = middle_;
            TopRight = topRight_;
        }

        public Visual(JSONNode jNode_)
        {
            Middle = jNode_["Middle"];
            TopRight = jNode_["TopRight"];
        }

        public static implicit operator Visual(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator JSONNode(Visual object_)
        {
            JSONObject jObject = new JSONObject();
            jObject["Middle"] = object_.Middle;
            jObject["TopRight"] = object_.TopRight;
            return jObject;
        }
    }
}
