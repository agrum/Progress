using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill
{ 
    public abstract partial class Numeric
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
            Metric,
        }

        public partial class ReferenceValue
        {
            private double value;

            public ReferenceValue(List<string> fields)
            {
                value = Convert.ToDouble(fields[1]);
            }
        }

        public partial class ReferenceUnitGauge : IReference
        {
            ESubject Subject;
            NamedHash Type;
            Unit.Gauge.EExtract Extract;

            public ReferenceUnitGauge(List<string> fields)
            {
                Subject = Serializer.ReadEnum<ESubject>(fields[1]);
                Type = fields[2];
                Extract = Serializer.ReadEnum<Unit.Gauge.EExtract>(fields[3]);
            }
        }

        public partial class ReferenceUnitStat : IReference
        {
            ESubject Subject;
            NamedHash Type;

            public ReferenceUnitStat(List<string> fields)
            {
                Subject = Serializer.ReadEnum<ESubject>(fields[1]);
                Type = fields[2];
            }
        }

        public partial class ReferencRandomRange : IReference
        {
            double A;
            double B;

            public ReferencRandomRange(List<string> fields)
            {
                A = Convert.ToDouble(fields[2]);
                B = Convert.ToDouble(fields[3]);
            }
        }

        public partial class ReferencModifier : IReference
        {
            ESubject Subject;
            uint Modifier;
            Modifier.EExtract Extract;

            public ReferencModifier(List<string> fields)
            {
                Subject = Serializer.ReadEnum<ESubject>(fields[1]);
                Modifier = Convert.ToUInt32(fields[3]);
                Extract = Serializer.ReadEnum<Modifier.EExtract>(fields[3]);
            }
        }

        public partial class ReferencInput : IReference
        {
            public ReferencInput(List<string> fields)
            {

            }
        }

        public partial class ReferenceCooldown : IReference
        {
            ESubject Subject;
            NamedHash Type;

            public ReferenceCooldown(List<string> fields)
            {
                Subject = Serializer.ReadEnum<ESubject>(fields[1]);
                Type = fields[2];
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
            return jNode_.Value;
        }

        public static implicit operator JSONNode(Numeric numeric_)
        {
            return new JSONString(numeric_.literal);
        }
    }

    public partial class NumericValue : Numeric
    {
        IReference reference;

        public NumericValue(string referenceString_)
        {
            var fields = new List<string>(referenceString_.Split('|'));
            EReferenceType referenceType = Serializer.ReadEnum<EReferenceType>(fields[0]);

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
    }

    public partial class NumericEquation : Numeric
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
    }
}
