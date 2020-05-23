using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Context.Skill.Modifier.Stacker
{
    public abstract class Base
    {
        public class Evolution
        {
            public uint Previous = 0;
            public uint Current = 0;
            public uint Added = 0;
            public uint Removed = 0;
        }

        public abstract uint Amount();
        public abstract double Expiration();
        public abstract void Add(uint amount_);
        public abstract void Remove(uint amount_);

        protected void _Alived(bool alive_)
        {
            Alived(alive_);
        }

        protected void _Changed(Evolution evolution_)
        {
            Changed(evolution_);
        }

        public delegate double DurationDelegate();
        public delegate uint MaxAmountDelegate();

        public delegate void AliveDelegate(bool alive_);
        public event AliveDelegate Alived = delegate { };

        public delegate void EvolutionDelegate(Evolution evolution_);
        public event EvolutionDelegate Changed = delegate { };
    }
}
