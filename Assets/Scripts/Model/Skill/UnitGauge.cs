using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Skill
{
    public class UnitGauge
    {
        public enum EInputType
        {
            Additive,
            AdditiveMultiplier,
            Multiplier,
        }

        public enum EExtract
        {
            Current,
            Ratio,
            Percentage,
            Max,
            Missing,
            MissingRatio,
            MissingPercentage
        }

        public NamedHash Type { get; private set; }
        private float Value = 0;
        private float Additive = 0;
        private float AdditiveMultiplier = 0;
        private float Multiplier = 1;

        public UnitGauge(NamedHash type_)
        {
            Type = type_;
        }

        public float Get(EExtract extract_)
        {
            switch (extract_)
            {
                case EExtract.Current: return Value;
                case EExtract.Ratio: return Value / GetMax();
                case EExtract.Percentage: return Value / GetMax() * 100.0f;
                case EExtract.Max: return GetMax();
                case EExtract.Missing: return GetMax() - Value;
                case EExtract.MissingRatio: return 1.0f - Value / GetMax();
                case EExtract.MissingPercentage: return (1.0f - Value / GetMax()) * 100.0f;
            }

            return 0.0f;
        }

        float GetMax()
        {
            return Additive * (1 + AdditiveMultiplier) * Multiplier;
        }
    }
}
