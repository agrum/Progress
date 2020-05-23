using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Context.Skill.Modifier.Stacker
{
    class Independent : Base
    {
        public class Grouped
        {
            public double expiration = 0;
            public uint amount = 0; 
        }

        private MaxAmountDelegate maxAmount;
        private DurationDelegate duration;
        private uint amount = 0;
        private double expiration = 0;
        private List<Grouped> groups = new List<Grouped>();

        public Independent(MaxAmountDelegate maxAmount_, DurationDelegate duration_)
        {
            maxAmount = maxAmount_;
            duration = duration_;
        }

        override public void Add(uint amount_)
        {
            if (amount_ == 0)
            {
                return;
            }

            var evolution = new Evolution() { Previous = amount, Added = amount_ };
            amount += amount_;
            groups.Add(new Grouped() { expiration = duration(), amount = amount_ });
            if (maxAmount() > 0 && amount > maxAmount())
            {
                evolution.Removed = amount - maxAmount();
                amount = maxAmount();
                RemoveFromGroups(evolution.Removed);
            }
            evolution.Current = amount;
            expiration = duration();
            if (evolution.Previous == 0)
            {
                _Alived(true);
            }
            _Changed(evolution);
        }

        override public uint Amount()
        {
            return amount;
        }

        override public double Expiration()
        {
            return expiration;
        }

        override public void Remove(uint amount_)
        {
            if (amount_ == 0)
            {
                return;
            }

            var evolution = new Evolution() { Previous = amount };
            if (amount >= amount_)
            {
                evolution.Removed = amount;
                amount = 0;
                expiration = 0;
                if (evolution.Previous != 0)
                {
                    _Alived(true);
                }
            }
            else
            {
                evolution.Removed = amount_;
                amount -= amount_;
            }
            RemoveFromGroups(evolution.Removed);
            evolution.Current = amount;
            _Changed(evolution);
        }

        void RemoveFromGroups(uint amount)
        {
            while (groups.Count > 0 && amount > 0)
            {
                if (groups[0].amount > amount)
                {
                    groups[0].amount -= amount;
                    amount = 0;
                }
                else
                {
                    amount -= groups[0].amount;
                    groups.RemoveAt(0);
                }
            }
        }
    }
}
