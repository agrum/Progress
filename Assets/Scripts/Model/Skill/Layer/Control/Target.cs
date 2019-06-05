using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Layer.Control
{
    public class Target : Base
    {
        [Flags]
        public enum ETargetType
        {
            None = 0,
            Self = 1,
            Ally = 2,
            Enemy = 4,
        }

        public ETargetType TargetType { get; private set; }
        public SkillMetricReference Radius { get; private set; }

        public Target(ETargetType targetType_, SkillMetricReference radius_)
        {
            TargetType = targetType_;
            Radius = radius_;
        }

        public Target(JSONNode jNode_)
        {
            TargetType = (ETargetType)Enum.Parse(typeof(ETargetType), jNode_["TargetType"]);
            Radius = jNode_["Radius"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["TargetType"] = TargetType.ToString("G");
            jObject["Radius"] = Radius;
            return jObject;
        }
    }
}
