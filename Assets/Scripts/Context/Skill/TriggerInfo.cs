using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.Skill
{
    public partial class MetricReference
    {
        public double Get(Context.Skill.TriggerInfo triggerInfo_)
        {
            if (Metric != null)
                return Metric.Numeric.Get(triggerInfo_);
            return Value;
        }
    }

    public abstract partial class Numeric
    {
        public interface IReference
        {
            double Get(Context.Skill.TriggerInfo triggerInfo_);
        }

        public partial class ReferenceValue : IReference
        {
            public double Get(Context.Skill.TriggerInfo triggerInfo_)
            {
                return value;
            }
        }

        public partial class ReferenceUnitGauge : IReference
        {
            public double Get(Context.Skill.TriggerInfo triggerInfo_)
            {
                return Subject.GetContainer(triggerInfo_).GetUnitGauge(Type).Get(Extract);
            }
        }

        public partial class ReferenceUnitStat : IReference
        {
            public double Get(Context.Skill.TriggerInfo triggerInfo_)
            {
                return Subject.GetContainer(triggerInfo_).GetUnitStat(Type).Value;
            }
        }

        public partial class ReferenceCooldown : IReference
        {
            public double Get(Context.Skill.TriggerInfo triggerInfo_)
            {
                return Subject.GetContainer(triggerInfo_).GetUnitStat(Type).Value;
            }
        }

        public partial class ReferencRandomRange : IReference
        {
            public double Get(Context.Skill.TriggerInfo triggerInfo_)
            {
                double rnd = (double)new Random().NextDouble();
                return A + (B - A) * rnd;
            }
        }

        public partial class ReferencModifier : IReference
        {
            public double Get(Context.Skill.TriggerInfo triggerInfo_)
            {
                return 1.0;
            }
        }

        public partial class ReferencInput : IReference
        {
            public double Get(Context.Skill.TriggerInfo triggerInfo_)
            {
                return triggerInfo_.Input;
            }
        }

        public abstract double Get(Context.Skill.TriggerInfo triggerInfo_);
    }

    public partial class NumericValue : Numeric
    {
        override public double Get(Context.Skill.TriggerInfo triggerInfo_)
        {
            return reference.Get(triggerInfo_);
        }
    }

    public partial class NumericEquation : Numeric
    {
        override public double Get(Context.Skill.TriggerInfo triggerInfo_)
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

namespace Assets.Scripts.Context.Skill
{
    public class TriggerInfo
    {
        public Container Target;
        public Container Source;
        public Container Trigger;
        public float Dt;
        public float Input;
    }
}
