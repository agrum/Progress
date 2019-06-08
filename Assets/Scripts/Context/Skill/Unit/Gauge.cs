using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Context.Skill.Unit
{
    public class Gauge
    {
        public Data.NamedHash Type { get; private set; }
        private float Value = 0;
        private float Additive = 0;
        private float AdditiveMultiplier = 0;
        private float Multiplier = 1;

        public Gauge(Data.NamedHash type_)
        {
            Type = type_;
        }

        public float Get(Data.Skill.Unit.Gauge.EExtract extract_)
        {
            switch (extract_)
            {
                case Data.Skill.Unit.Gauge.EExtract.Current: return Value;
                case Data.Skill.Unit.Gauge.EExtract.Ratio: return Value / GetMax();
                case Data.Skill.Unit.Gauge.EExtract.Percentage: return Value / GetMax() * 100.0f;
                case Data.Skill.Unit.Gauge.EExtract.Max: return GetMax();
                case Data.Skill.Unit.Gauge.EExtract.Missing: return GetMax() - Value;
                case Data.Skill.Unit.Gauge.EExtract.MissingRatio: return 1.0f - Value / GetMax();
                case Data.Skill.Unit.Gauge.EExtract.MissingPercentage: return (1.0f - Value / GetMax()) * 100.0f;
            }

            return 0.0f;
        }

        float GetMax()
        {
            return Additive * (1 + AdditiveMultiplier) * Multiplier;
        }
    }
}
