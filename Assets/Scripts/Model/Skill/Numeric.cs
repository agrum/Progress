using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using Assets.Scripts.Model.UnitAttribute;

namespace Assets.Scripts.Model.Skill
{
    public class Numeric
    {
        enum ESubject
        {
            Source,
            Target,
            Trigger,
            Parent
        }

        enum EExtract
        {
            Current,
            Ratio,
            Percentage,
            Max,
            Missing,
            MissingRatio,
            MissingPercentage
        }

        public enum EReferenceType
        {
            UnitGauge,
            UnitStat,
            RandomRange,
            Modifier,
            Input,
        }

        public interface IReference
        {
            float Get();
            EReferenceType ReferenceType();
            JSONNode ToJson();
        }

        public class ReferenceUnitGauge : IReference
        {
            ESubject Subject;
            UnitGauge.EType Type;
            UnitGauge.EExtract Extract;

            public ReferenceUnitGauge(JSONNode jNode_)
            {
                Subject = (ESubject)jNode_["subject"].AsInt;
                Type = (UnitGauge.EType)jNode_["type"].AsInt;
                Extract = (UnitGauge.EExtract)jNode_["extract"].AsInt;
            }

            public float Get() { return 0; }

            public EReferenceType ReferenceType() { return EReferenceType.UnitGauge; }

            public JSONNode ToJson()
            {
                JSONObject jObject = new JSONObject();
                jObject["subject"] = (int)Subject;
                jObject["type"] = (int)Type;
                jObject["extract"] = (int)Extract;
                return jObject;
            }
        }

        public class ReferenceUnitStat : IReference
        {
            ESubject Subject;
            UnitStat.EType Type;

            public ReferenceUnitStat(JSONNode jNode_)
            {
                Subject = (ESubject)jNode_["subject"].AsInt;
                Type = (UnitStat.EType)jNode_["type"].AsInt;
            }

            public float Get() { return 0; }

            public EReferenceType ReferenceType() { return EReferenceType.UnitStat; }

            public JSONNode ToJson()
            {
                JSONObject jObject = new JSONObject();
                jObject["subject"] = (int)Subject;
                jObject["type"] = (int)Type;
                return jObject;
            }
        }

        public class ReferencRandomRange : IReference
        {
            Numeric NumericA;
            Numeric NumericB;

            public ReferencRandomRange(JSONNode jNode_)
            {
                NumericA = new Numeric(jNode_["numericA"]);
                NumericB = new Numeric(jNode_["numericB"]);
            }

            public float Get() { return 0; }

            public EReferenceType ReferenceType() { return EReferenceType.RandomRange; }

            public JSONNode ToJson()
            {
                JSONObject jObject = new JSONObject();
                jObject["numericA"] = NumericA;
                jObject["numericA"] = NumericB;
                return jObject;
            }
        }

        public class ReferencModifier : IReference
        {
            ESubject Subject;
            uint Modifier;

            public ReferencModifier(JSONNode jNode_)
            {
                Subject = (ESubject)jNode_["subject"].AsInt;
                Modifier = (uint)jNode_["modifier"].AsInt;
            }

            public float Get() { return 0; }

            public EReferenceType ReferenceType() { return EReferenceType.Modifier; }

            public JSONNode ToJson()
            {
                JSONObject jObject = new JSONObject();
                jObject["subject"] = (int)Subject;
                jObject["modifier"] = (int)Modifier;
                return jObject;
            }
        }

        public class ReferencInput : IReference
        {
            public ReferencInput()
            {

            }

            public ReferencInput(JSONNode jNode_)
            {

            }

            public float Get() { return 0; }

            public EReferenceType ReferenceType() { return EReferenceType.Input; }

            public JSONNode ToJson()
            {
                return 1;
            }
        }

        float Base;
        IReference Reference;

        Numeric(float base_)
        {
            Base = base_;
            Reference = null;
        }

        Numeric(float base_, IReference reference_)
        {
            Base = base_;
            Reference = reference_;
        }

        Numeric(JSONNode jNode_)
        {
            if (jNode_.IsNumber)
            {
                Base = jNode_;
                Reference = null;
            }
            else
            {
                Base = jNode_["base"];
                if (!jNode_["referenceType"].IsNull)
                {
                    switch ((EReferenceType)jNode_["referenceType"].AsInt)
                    {
                        case EReferenceType.Input: Reference = new ReferencInput(jNode_["reference"]); break;
                        case EReferenceType.Modifier: Reference = new ReferencModifier(jNode_["reference"]); break;
                        case EReferenceType.RandomRange: Reference = new ReferencRandomRange(jNode_["reference"]); break;
                        case EReferenceType.UnitGauge: Reference = new ReferenceUnitGauge(jNode_["reference"]); break;
                        case EReferenceType.UnitStat: Reference = new ReferenceUnitStat(jNode_["reference"]); break;
                    }
                }
            }
        }

        public static implicit operator Numeric(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator JSONNode(Numeric numeric_)
        {
            if (numeric_.Reference == null)
            {
                return numeric_.Base;
            }
            else
            { 
                JSONObject jObject = new JSONObject();
                jObject["base"] = numeric_.Base;
                jObject["referenceType"] = (int) numeric_.Reference.ReferenceType();
                jObject["reference"] = numeric_.Reference.ToJson();
                return jObject;
            }
        }
    }
}
