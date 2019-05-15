using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Skill
{
    public class UnitStat
    {
        public enum EInputType
        {
            Additive,
            AdditiveMultiplier,
            Multiplier,
        }

        public NamedHash Type { get; private set; }
        private float Additive = 0;
        private float AdditiveMultiplier = 0;
        private float Multiplier = 1;
        private float Zeroes = 0;
        public float Value { get; private set; } = 0;

        public UnitStat(NamedHash type_)
        {
            Type = type_;
        }

        public void Input(float value_, EInputType type_)
        {
            switch (type_)
            {
                case EInputType.Additive:
                    Additive += value_; break;
                case EInputType.AdditiveMultiplier:
                    AdditiveMultiplier += value_; break;
                case EInputType.Multiplier:
                    if (value_ == 0)
                        ++Zeroes;
                    else if (float.IsInfinity(value_))
                        --Zeroes;
                    else 
                        Multiplier += value_;
                    break;
            }

            Value = Additive * (1.0f + AdditiveMultiplier) * Multiplier * (Zeroes > 0 ? 0 : 1);
        }
    }
}
