using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.Skill.Unit
{
    public class Gauge
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
    }
}
