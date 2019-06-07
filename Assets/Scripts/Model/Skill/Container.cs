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

        public Unit.Stat GetUnitStat(NamedHash type_)
        {
            Unit.Stat rtn;
            if (!_UnitStats.TryGetValue(type_, out rtn))
            {
                rtn = new Unit.Stat(type_);
                _UnitStats.Add(type_, rtn);
            }
            return rtn;
        }

        public Unit.Gauge GetUnitGauge(NamedHash type_)
        {
            Unit.Gauge rtn;
            if (!_UnitGauges.TryGetValue(type_, out rtn))
            {
                rtn = new Unit.Gauge(type_);
                _UnitGauges.Add(type_, rtn);
            }
            return rtn;
        }

        public Unit.Cooldown GetCooldown(NamedHash type_)
        {
            Unit.Cooldown rtn;
            if (!_Cooldowns.TryGetValue(type_, out rtn))
            {
                rtn = new Unit.Cooldown(type_);
                _Cooldowns.Add(type_, rtn);
            }
            return rtn;
        }

        public SortedList<NamedHash, Unit.Stat> _UnitStats = new SortedList<NamedHash, Unit.Stat>(new NamedHash.TypeComparer());
        public SortedList<NamedHash, Unit.Gauge> _UnitGauges = new SortedList<NamedHash, Unit.Gauge>(new NamedHash.TypeComparer());
        public SortedList<NamedHash, Unit.Cooldown> _Cooldowns = new SortedList<NamedHash, Unit.Cooldown>(new NamedHash.TypeComparer());
    }
}
