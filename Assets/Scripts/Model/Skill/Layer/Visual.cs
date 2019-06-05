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
        public SkillMetricReference Middle { get; private set; }
        public SkillMetricReference TopRight { get; private set; }

        public Visual(SkillMetricReference middle_)
        {
            Middle = middle_;
        }

        public Visual(SkillMetricReference middle_, SkillMetricReference topRight_)
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
