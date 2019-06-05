using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill
{ 
    public class Numeric
    {
        private string literal;

        public enum EReferenceType
        {
            Value,
            UnitGauge,
            UnitStat,
            RandomRange,
            Modifier,
            Input,
            Cooldown,
        }

        public interface IReference
        {
            double Get(TriggerInfo triggerInfo_);
        }

        public class ReferenceValue : IReference
        {
            private double value;

            public ReferenceValue(List<string> fields)
            {
                value = Convert.ToDouble(fields[1]);
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                return value;
            }
        }

        public class ReferenceUnitGauge : IReference
        {
            ESubject Subject;
            NamedHash Type;
            UnitGauge.EExtract Extract;

            public ReferenceUnitGauge(List<string> fields)
            {
                Enum.TryParse(fields[1], true, out Subject);
                Type = fields[2];
                Enum.TryParse(fields[3], true, out Extract);
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                return Subject.GetContainer(triggerInfo_).GetUnitGauge(Type).Get(Extract);
            }
        }

        public class ReferenceUnitStat : IReference
        {
            ESubject Subject;
            NamedHash Type;

            public ReferenceUnitStat(List<string> fields)
            {
                Enum.TryParse(fields[1], true, out Subject);
                Type = fields[2];
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                return Subject.GetContainer(triggerInfo_).GetUnitStat(Type).Value;
            }
        }

        public class ReferencRandomRange : IReference
        {
            double A;
            double B;

            public ReferencRandomRange(List<string> fields)
            {
                A = Convert.ToDouble(fields[2]);
                B = Convert.ToDouble(fields[3]);
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                double rnd = (double)new Random().NextDouble();
                return A + (B - A) * rnd;
            }
        }

        public class ReferencModifier : IReference
        {
            ESubject Subject;
            uint Modifier;
            Modifier.EExtract Extract;

            public ReferencModifier(List<string> fields)
            {
                Enum.TryParse(fields[1], true, out Subject);
                Modifier = Convert.ToUInt32(fields[3]);
                Enum.TryParse(fields[3], true, out Extract);
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                return 1.0;
            }
        }

        public class ReferencInput : IReference
        {
            public ReferencInput(List<string> fields)
            {

            }

            public double Get(TriggerInfo triggerInfo_)
            {
                return triggerInfo_.Input;
            }
        }

        public class ReferenceCooldown : IReference
        {
            ESubject Subject;
            NamedHash Type;

            public ReferenceCooldown(List<string> fields)
            {
                Enum.TryParse(fields[1], true, out Subject);
                Type = fields[2];
            }

            public double Get(TriggerInfo triggerInfo_)
            {
                return Subject.GetContainer(triggerInfo_).GetUnitStat(Type).Value;
            }
        }

        public static implicit operator Numeric(List<string> fields_)
        {
            if (fields_.Count() % 2 != 1)
                throw new InvalidOperationException();

            while (fields_.First() == "(" && fields_.Last() == ")")
                fields_ = fields_.GetRange(1, fields_.Count() - 2);

            Numeric numeric;
            if (fields_.Count() == 1)
                numeric = new NumericValue(fields_.First());
            else
                numeric = new NumericEquation(fields_);
            numeric.literal = string.Join(" ", fields_);

            return numeric;
        }

        public static implicit operator Numeric(string literal_)
        {
            return new List<string>(literal_.Split(' '));
        }

        public static implicit operator Numeric(JSONNode jNode_)
        {
            return jNode_.ToString();
        }

        public static implicit operator JSONNode(Numeric numeric_)
        {
            return numeric_.literal;
        }

        public virtual double Get(TriggerInfo triggerInfo_) { return 0.0; }
    }

    public class NumericValue : Numeric
    {
        IReference reference;

        public NumericValue(string referenceString_)
        {
            var fields = new List<string>(referenceString_.Split('|'));
            EReferenceType referenceType;
            if (!Enum.TryParse(fields[0], true, out referenceType))
            {
                throw new InvalidOperationException();
            }

            switch (referenceType)
            {
                case EReferenceType.Value:
                    reference = new ReferenceValue(fields);
                    break;
                case EReferenceType.UnitGauge:
                    reference = new ReferenceUnitGauge(fields);
                    break;
                case EReferenceType.UnitStat:
                    reference = new ReferenceUnitStat(fields);
                    break;
                case EReferenceType.RandomRange:
                    reference = new ReferencRandomRange(fields);
                    break;
                case EReferenceType.Modifier:
                    reference = new ReferencModifier(fields);
                    break;
                case EReferenceType.Input:
                    reference = new ReferencInput(fields);
                    break;
            }
        }

        override public double Get(TriggerInfo triggerInfo_)
        {
            return reference.Get(triggerInfo_);
        }
    }

    public class NumericEquation : Numeric
    {
        enum EOperator
        {
            Add,
            Sub,
            Mul,
            Div,
        }

        private EOperator ope;
        private Numeric left;
        private Numeric right;

        public NumericEquation(List<string> fields)
        {
            bool strongOperatorFound = false;
            int operatorPosition = fields.Count();
            int parenthesisScopeCount = 0;

            for (int i = fields.Count() - 1; i >= 0; --i)
            {
                if (fields[i] == ")")
                    ++parenthesisScopeCount;
                else if (fields[i] == "(")
                    --parenthesisScopeCount;
                else if (parenthesisScopeCount == 0)
                {
                    if (fields[i] == "+" || fields[i] == "-")
                    {
                        operatorPosition = i;
                        break;
                    }
                    if (!strongOperatorFound && (fields[i] == "*" || fields[i] == "/"))
                    {
                        strongOperatorFound = true;
                        operatorPosition = i;
                    }
                }
            }
            left = fields.GetRange(0, operatorPosition);
            right = fields.GetRange(operatorPosition + 1, fields.Count() - 1 - operatorPosition);
            if (fields[operatorPosition] == "+")
                ope = EOperator.Add;
            else if (fields[operatorPosition] == "-")
                ope = EOperator.Sub;
            else if (fields[operatorPosition] == "*")
                ope = EOperator.Mul;
            else if (fields[operatorPosition] == "/")
                ope = EOperator.Div;
        }

        override public double Get(TriggerInfo triggerInfo_)
        {
            switch (ope)
            {
                case EOperator.Add: return left.Get(triggerInfo_) + right.Get(triggerInfo_);
                case EOperator.Sub: return left.Get(triggerInfo_) - right.Get(triggerInfo_);
                case EOperator.Mul: return left.Get(triggerInfo_) * right.Get(triggerInfo_);
                case EOperator.Div: return left.Get(triggerInfo_) / right.Get(triggerInfo_);
            }

            throw new InvalidOperationException();
        }
    }
}
