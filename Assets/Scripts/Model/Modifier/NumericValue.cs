using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model
{
    public abstract class INumericValue
    {
        public abstract float Get();

        static INumericValue Make(SkillMetric metric_)
        {
            if (true)
            {
                return new NumericValueFlat(metric_.Value);
            }
        }
    }

    public class NumericValueFlat : INumericValue
    {
        private float value;

        public NumericValueFlat(float value_)
        {
            value = value_;
        }

        public override float Get()
        {
            return value;
        }
    }
}
