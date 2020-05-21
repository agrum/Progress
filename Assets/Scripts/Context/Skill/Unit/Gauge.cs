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
        private Stat Capacity;
        private float Value;

        public Gauge(Data.NamedHash type_)
        {
            Type = type_;
            Capacity = new Stat(type_);
        }

        public float Get(Data.Skill.Unit.Gauge.EExtract extract_)
        {
            switch (extract_)
            {
                case Data.Skill.Unit.Gauge.EExtract.Current: return Value;
                case Data.Skill.Unit.Gauge.EExtract.Ratio: return Ratio();
                case Data.Skill.Unit.Gauge.EExtract.Percentage: return 100 * Ratio();
                case Data.Skill.Unit.Gauge.EExtract.Max: return Capacity.Value;
                case Data.Skill.Unit.Gauge.EExtract.Missing: return Capacity.Value - Value;
                case Data.Skill.Unit.Gauge.EExtract.MissingRatio: return 1.0f - Ratio();
                case Data.Skill.Unit.Gauge.EExtract.MissingPercentage: return 100.0f * (1.0f - Ratio());
            }

            return 0.0f;
        }

        float Ratio()
        {
            return Capacity.Value > 0 ? Value / Capacity.Value : 1;
        }
    }
}
