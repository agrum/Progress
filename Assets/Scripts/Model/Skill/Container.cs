using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Skill
{
    public class Container
    {
        public Container Parent { get; private set; }
        public SortedList<UnitStat.EType, UnitStat> UnitStats;
        public SortedList<UnitGauge.EType, UnitGauge> UnitGauges;
    }
}
