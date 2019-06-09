using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
{
    public class Physics : Base
    {
        public enum ETravelType
        {
            DurationBased,
            SpeedBased,
        }

        public EDirection BaseDirection;
        public MetricReference AddedDirection;
        public bool Normalized;
        public MetricReference Magnitude;
        public ETravelType TravelType;
        public MetricReference TravelParameter;

        public Physics(
            Id id_,
            EDirection baseDirection,
            MetricReference addedDirection_,
            bool normalized_,
            MetricReference magnitude_,
            ETravelType travelType_,
            MetricReference travelParameter_)
            : base(id_)
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
            BaseDirection = Serializer.ReadEnum<EDirection>(jNode_["baseDirection"]);
            AddedDirection = jNode_["addedDirection"];
            Normalized = jNode_["normalized"].AsBool;
            Magnitude = jNode_["magnitude"];
            TravelType = Serializer.ReadEnum<ETravelType>(jNode_["travelType"]);
            TravelParameter = jNode_["travelParameter"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["baseDirection"] = BaseDirection;
            jObject["addedDirection"] = AddedDirection;
            jObject["normalized"] = Normalized;
            jObject["magnitude"] = Magnitude;
            jObject["travelType"] = TravelType;
            jObject["travelParameter"] = TravelParameter;
            return jObject;
        }
    }
}
