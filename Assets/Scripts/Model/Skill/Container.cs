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

        public UnitStat GetUnitStat(UnitStat.EType type_)
        {
            UnitStat rtn;
            if (!_UnitStats.TryGetValue(type_, out rtn))
            {
                rtn = new UnitStat(type_);
                _UnitStats.Add(type_, rtn);
            }
            return rtn;
        }

        public UnitGauge GetUnitGauge(UnitGauge.EType type_)
        {
            UnitGauge rtn;
            if (!_UnitGauges.TryGetValue(type_, out rtn))
            {
                rtn = new UnitGauge(type_);
                _UnitGauges.Add(type_, rtn);
            }
            return rtn;
        }

        public SortedList<UnitStat.EType, UnitStat> _UnitStats;
        public SortedList<UnitGauge.EType, UnitGauge> _UnitGauges;
    }
}
