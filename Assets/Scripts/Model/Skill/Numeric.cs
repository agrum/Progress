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
            double Get(TriggerInfo triggerInfo_);
            EReferenceType ReferenceType();
            JSONNode ToJson();
        }

        public class ReferenceUnitGauge : IReference
        {
            ESubject Subject;
            UnitGauge.EType Type;
            UnitGauge.EExtract Extract;
            EEquation Equation;

            public ReferenceUnitGauge(JSONNode jNode_)
            {
                Subject = (ESubject)jNode_["subject"].AsInt;
                Type = (UnitGauge.EType)jNode_["type"].AsInt;
                Extract = (UnitGauge.EExtract)jNode_["extract"].AsInt;
                Equation = (EEquation)jNode_["equation"].AsInt;
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                return Equation.Compute(Subject.GetContainer(triggerInfo_).GetUnitGauge(Type).Get(Extract));
            }

            public EReferenceType ReferenceType() { return EReferenceType.UnitGauge; }

            public JSONNode ToJson()
            {
                JSONObject jObject = new JSONObject();
                jObject["subject"] = (int)Subject;
                jObject["type"] = (int)Type;
                jObject["extract"] = (int)Extract;
                jObject["equation"] = (int)Equation;
                return jObject;
            }
        }

        public class ReferenceUnitStat : IReference
        {
            ESubject Subject;
            UnitStat.EType Type;
            EEquation Equation;

            public ReferenceUnitStat(JSONNode jNode_)
            {
                Subject = (ESubject)jNode_["subject"].AsInt;
                Type = (UnitStat.EType)jNode_["type"].AsInt;
                Equation = (EEquation)jNode_["equation"].AsInt;
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                return Equation.Compute(Subject.GetContainer(triggerInfo_).GetUnitStat(Type).Value);
            }

            public EReferenceType ReferenceType() { return EReferenceType.UnitStat; }

            public JSONNode ToJson()
            {
                JSONObject jObject = new JSONObject();
                jObject["subject"] = (int)Subject;
                jObject["type"] = (int)Type;
                jObject["equation"] = (int)Equation;
                return jObject;
            }
        }

        public class ReferencRandomRange : IReference
        {
            Numeric NumericA;
            Numeric NumericB;
            EEquation Equation;

            public ReferencRandomRange(JSONNode jNode_)
            {
                NumericA = new Numeric(jNode_["numericA"]);
                NumericB = new Numeric(jNode_["numericB"]);
                Equation = (EEquation)jNode_["equation"].AsInt;
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                double a = NumericA.Get(triggerInfo_);
                double b = NumericB.Get(triggerInfo_);
                double rnd = (double) new Random().NextDouble();
                return Equation.Compute(a + (b - a) * rnd);
            }

            public EReferenceType ReferenceType() { return EReferenceType.RandomRange; }

            public JSONNode ToJson()
            {
                JSONObject jObject = new JSONObject();
                jObject["numericA"] = NumericA;
                jObject["numericA"] = NumericB;
                jObject["equation"] = (int)Equation;
                return jObject;
            }
        }

        public class ReferencModifier : IReference
        {
            ESubject Subject;
            uint Modifier;
            EEquation Equation;

            public ReferencModifier(JSONNode jNode_)
            {
                Subject = (ESubject)jNode_["subject"].AsInt;
                Modifier = (uint)jNode_["modifier"].AsInt;
                Equation = (EEquation)jNode_["equation"].AsInt;
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                return Equation.Compute(1.0);
            }

            public EReferenceType ReferenceType() { return EReferenceType.Modifier; }

            public JSONNode ToJson()
            {
                JSONObject jObject = new JSONObject();
                jObject["subject"] = (int)Subject;
                jObject["modifier"] = (int)Modifier;
                jObject["equation"] = (int)Equation;
                return jObject;
            }
        }

        public class ReferencInput : IReference
        {
            EEquation Equation;

            public ReferencInput(JSONNode jNode_)
            {
                Equation = (EEquation)jNode_["equation"].AsInt;
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                return Equation.Compute(triggerInfo_.Input);
            }

            public EReferenceType ReferenceType() { return EReferenceType.Input; }

            public JSONNode ToJson()
            {
                JSONObject jObject = new JSONObject();
                jObject["equation"] = (int)Equation;
                return jObject;
            }
        }

        double Base;
        IReference Reference;

        Numeric(double base_)
        {
            Base = base_;
            Reference = null;
        }

        Numeric(double base_, IReference reference_)
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

        public double Get(TriggerInfo triggerInfo_)
        {
            return 0;
        }
    }
}
