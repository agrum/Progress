using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill
{
    public class NumericBuilder
    {
        private uint parenthesisOpened = 0;
        private bool waitingOnField = true;
        private string equationString;

        public NumericBuilder()
        {
        }

        public Numeric Validate()
        {
            if (parenthesisOpened != 0 || waitingOnField)
                throw new InvalidOperationException();

            if (equationString.First() == ' ')
                equationString.Remove(0);

            return equationString;
        }

        public NumericBuilder OpenP()
        {
            equationString += " (";
            ++parenthesisOpened;

            return this;
        }

        public NumericBuilder CloseP()
        {
            equationString += " )";
            if (parenthesisOpened == 0 || waitingOnField == true)
            {
                throw new InvalidOperationException();
            }
            --parenthesisOpened;
            waitingOnField = true;

            return this;
        }

        private NumericBuilder OperandAdded()
        {
            if (waitingOnField)
            {
                throw new InvalidOperationException();
            }
            waitingOnField = true;

            return this;
        }

        public NumericBuilder Add()
        {
            equationString += " +";
            return OperandAdded();
        }

        public NumericBuilder Sub()
        {
            equationString += " -";
            return OperandAdded();
        }

        public NumericBuilder Mul()
        {
            equationString += " *";
            return OperandAdded();
        }

        public NumericBuilder Div()
        {
            equationString += " /";
            return OperandAdded();
        }

        private NumericBuilder FieldAdded()
        {
            if (!waitingOnField)
            {
                throw new InvalidOperationException();
            }
            waitingOnField = false;

            return this;
        }

        public NumericBuilder Value(double value_)
        {
            equationString +=
                " " + Serializer.WriteEnum(Numeric.EReferenceType.Value) +
                "|" + value_.ToString();
            return FieldAdded();
        }

        public NumericBuilder Random(double min_, double max_)
        {
            equationString +=
                " " + Serializer.WriteEnum(Numeric.EReferenceType.RandomRange) +
                "|" + min_.ToString() +
                "|" + max_.ToString();
            return FieldAdded();
        }

        public NumericBuilder Gauge(ESubject subject_, string type_, Unit.Gauge.EExtract extract_)
        {
            equationString += 
                " " + Serializer.WriteEnum(Numeric.EReferenceType.UnitGauge) + 
                "|" + subject_.ToString("G") +
                "|" + type_ + 
                "|" + extract_.ToString("G");
            return FieldAdded();
        }

        public NumericBuilder Stat(ESubject subject_, string type_)
        {
            equationString += 
                " " + Serializer.WriteEnum(Numeric.EReferenceType.UnitStat) + 
                "|" + subject_.ToString("G") + 
                "|" + type_;
            return FieldAdded();
        }

        public NumericBuilder Modifier(ESubject subject_, uint modifier_, Modifier.EExtract extract_)
        {
            equationString += 
                " " + Serializer.WriteEnum(Numeric.EReferenceType.Modifier) +
                "|" + subject_.ToString("G") + 
                "|" + modifier_.ToString() + 
                "|" + extract_.ToString("G");
            return FieldAdded();
        }

        public NumericBuilder Input()
        {
            equationString +=
                " " + Serializer.WriteEnum(Numeric.EReferenceType.Input);
            return FieldAdded();
        }

        public NumericBuilder Cooldown(ESubject subject_, string name_)
        {
            equationString +=
                " " + Serializer.WriteEnum(Numeric.EReferenceType.Input) +
                "|" + subject_.ToString("G") +
                "|" + name_;
            return FieldAdded();
        }
    }
}
