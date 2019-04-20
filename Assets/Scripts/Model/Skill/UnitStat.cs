using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Skill
{
    public class UnitStat
    {
        public enum EType
        {
            CriticalProbability,
            Haste,
            Armor,
            Speed,
            HealthRegeneration,
            StaminaRegeneration,
        }

        public enum EInputType
        {
            Additive,
            AdditiveMultiplier,
            Multiplier,
        }

        private float Additive = 0;
        private float AdditiveMultiplier = 0;
        private float Multiplier = 1;
        private float Zeroes = 0;
        private float Computed = 0;

        public float Get()
        {
            return Computed;
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

            Computed = Additive * (1.0f + AdditiveMultiplier) * Multiplier * (Zeroes > 0 ? 0 : 1);
        }
    }
}
