using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Skill
{
    public class UnitGauge
    {
        public enum EType
        {
            Health,
            Energy,
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
