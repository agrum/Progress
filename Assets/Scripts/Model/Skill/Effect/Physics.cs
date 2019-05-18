using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class Physics : Base
    {
        public enum EDirection
        {
            SourceAim,
            TargetAim,
            TriggerAim,
            SourceToTarget,
            SourceToTrigger,
            TargetToSource,
            TargetToTrigger,
            TriggerToSource,
            TriggerToTarget,
        }

        public enum ETravelType
        {
            DurationBased,
            SpeedBased,
        }

        public EDirection BaseDirection;
        public SkillMetricReference AddedDirection;
        public bool Normalized;
        public SkillMetricReference Magnitude;
        public ETravelType TravelType;
        public SkillMetricReference TravelParameter;

        public Physics(
            string name_,
            EDirection baseDirection,
            SkillMetricReference addedDirection_,
            bool normalized_,
            SkillMetricReference magnitude_,
            ETravelType travelType_,
            SkillMetricReference travelParameter_,
            ESubject from_ = ESubject.Trigger,
            ESubject to_ = ESubject.Target)
            : base(name_, from_, to_)
        {
            BaseDirection = baseDirection;
            AddedDirection = addedDirection_;
            Normalized = normalized_;
            Magnitude = magnitude_;
            TravelType = travelType_;
            TravelParameter = travelParameter_;
        }

        public Physics(
            JSONNode jNode_)
            : base(jNode_)
        {
            BaseDirection = (EDirection)Enum.Parse(typeof(EDirection), jNode_["BaseDirection"]);
            AddedDirection = jNode_["AddedDirection"];
            Normalized = jNode_["Normalized"].AsBool;
            Magnitude = jNode_["Magnitude"];
            TravelType = (ETravelType)Enum.Parse(typeof(ETravelType), jNode_["TravelType"]);
            TravelParameter = jNode_["TravelParameter"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["BaseDirection"] = BaseDirection.ToString("G");
            jObject["AddedDirection"] = AddedDirection;
            jObject["Normalized"] = Normalized;
            jObject["Magnitude"] = Magnitude;
            jObject["TravelType"] = TravelType.ToString("G");
            jObject["TravelParameter"] = TravelParameter;
            return jObject;
        }
    }
}
