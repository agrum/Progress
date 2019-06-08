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
            BaseDirection = Serializer.ReadEnum<EDirection>(jNode_["BaseDirection"]);
            AddedDirection = jNode_["AddedDirection"];
            Normalized = jNode_["Normalized"].AsBool;
            Magnitude = jNode_["Magnitude"];
            TravelType = Serializer.ReadEnum<ETravelType>(jNode_["TravelType"]);
            TravelParameter = jNode_["TravelParameter"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["BaseDirection"] = BaseDirection;
            jObject["AddedDirection"] = AddedDirection;
            jObject["Normalized"] = Normalized;
            jObject["Magnitude"] = Magnitude;
            jObject["TravelType"] = TravelType;
            jObject["TravelParameter"] = TravelParameter;
            return jObject;
        }
    }
}
