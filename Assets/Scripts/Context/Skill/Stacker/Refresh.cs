using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Context.Skill.Stacker
{
    public class Refresh : Base
    {
        private MaxAmountDelegate maxAmount;
        private DurationDelegate duration;
        Utility.Scheduler scheduler;
        IEnumerator coroutine = null;
        private uint amount = 0;
        private double expiration = 0;

        public Refresh(MaxAmountDelegate maxStack_, DurationDelegate duration_, Utility.Scheduler scheduler_)
        {
            maxAmount = maxStack_;
            duration = duration_;
            scheduler = scheduler_;
        }

        override public void Add(uint amount_)
        {
            if (amount_ == 0)
            {
                return;
            }

            var evolution = new Evolution() { Previous = amount, Added = amount_ };
            amount += amount_;
            if (maxAmount() > 0 && amount > maxAmount())
            {
                evolution.Removed = amount - maxAmount();
                amount = maxAmount();
            }
            evolution.Current = amount;
            expiration = scheduler.Now + duration();
            ScheduleRefresh();
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
                if (coroutine != null)
                {
                    scheduler.StopCoroutine(coroutine);
                    coroutine = null;
                }
            }
            else
            {
                evolution.Removed = amount_;
                amount -= amount_;
            }
            evolution.Current = amount;
            _Changed(evolution);
        }

        void ScheduleRefresh()
        {
            if (coroutine != null)
            {
                scheduler.StopCoroutine(coroutine);
            }
            coroutine = scheduler.WaitUntil(Expiration()));
            scheduler.StartCoroutine(coroutine);
            coroutine = null;

            Remove(Amount());
        }
    }
}
