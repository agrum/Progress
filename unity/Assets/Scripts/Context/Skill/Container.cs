using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Context.Skill
{
    public class Container
    {
        public Container Parent { get; private set; }

        public Unit.Stat GetUnitStat(Data.NamedHash type_)
        {
            Unit.Stat rtn;
            if (!_UnitStats.TryGetValue(type_, out rtn))
            {
                rtn = new Unit.Stat(type_);
                _UnitStats.Add(type_, rtn);
            }
            return rtn;
        }

        public Unit.Gauge GetUnitGauge(Data.NamedHash type_)
        {
            Unit.Gauge rtn;
            if (!_UnitGauges.TryGetValue(type_, out rtn))
            {
                rtn = new Unit.Gauge(type_);
                _UnitGauges.Add(type_, rtn);
            }
            return rtn;
        }

        public Unit.Cooldown GetCooldown(Data.NamedHash type_)
        {
            Unit.Cooldown rtn;
            if (!_Cooldowns.TryGetValue(type_, out rtn))
            {
                rtn = new Unit.Cooldown(type_);
                _Cooldowns.Add(type_, rtn);
            }
            return rtn;
        }

        public SortedList<Data.NamedHash, Unit.Stat> _UnitStats = new SortedList<Data.NamedHash, Unit.Stat>(new Data.NamedHash.TypeComparer());
        public SortedList<Data.NamedHash, Unit.Gauge> _UnitGauges = new SortedList<Data.NamedHash, Unit.Gauge>(new Data.NamedHash.TypeComparer());
        public SortedList<Data.NamedHash, Unit.Cooldown> _Cooldowns = new SortedList<Data.NamedHash, Unit.Cooldown>(new Data.NamedHash.TypeComparer());
    }
}
