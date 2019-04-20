using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Skill
{
    public enum EEquation
    {
        Multiply,
        Inverse,
        SafeInverse,
    }

    static class EquationMethods
    {
        public static double Compute(this EEquation equation_, double value_)
        {
            switch (equation_)
            {
                case EEquation.Multiply: return value_;
                case EEquation.Inverse: return 1.0 / value_;
                case EEquation.SafeInverse: return 1.0 / (value_ + 1.0);
            }

            return value_;
        }
    }
}
