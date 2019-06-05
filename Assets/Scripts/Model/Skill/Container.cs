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

        public UnitStat GetUnitStat(NamedHash type_)
        {
            UnitStat rtn;
            if (!_UnitStats.TryGetValue(type_, out rtn))
            {
                rtn = new UnitStat(type_);
                _UnitStats.Add(type_, rtn);
            }
            return rtn;
        }

        public UnitGauge GetUnitGauge(NamedHash type_)
        {
            UnitGauge rtn;
            if (!_UnitGauges.TryGetValue(type_, out rtn))
            {
                rtn = new UnitGauge(type_);
                _UnitGauges.Add(type_, rtn);
            }
            return rtn;
        }

        public UnitCooldown GetCooldown(NamedHash type_)
        {
            UnitCooldown rtn;
            if (!_Cooldowns.TryGetValue(type_, out rtn))
            {
                rtn = new UnitCooldown(type_);
                _Cooldowns.Add(type_, rtn);
            }
            return rtn;
        }

        public SortedList<NamedHash, UnitStat> _UnitStats = new SortedList<NamedHash, UnitStat>(new NamedHash.TypeComparer());
        public SortedList<NamedHash, UnitGauge> _UnitGauges = new SortedList<NamedHash, UnitGauge>(new NamedHash.TypeComparer());
        public SortedList<NamedHash, UnitCooldown> _Cooldowns = new SortedList<NamedHash, UnitCooldown>(new NamedHash.TypeComparer());
    }
}
