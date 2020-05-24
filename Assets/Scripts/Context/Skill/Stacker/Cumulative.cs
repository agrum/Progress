using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Context.Skill.Stacker
{
    class Cumulative : Base
    {
        MaxAmountDelegate maxAmount;
        DurationDelegate duration;
        Utility.Scheduler scheduler;
        IEnumerator coroutine = null;
        uint amount = 0;
        double expiration = 0;

        public Cumulative(MaxAmountDelegate maxAmount_, DurationDelegate duration_, Utility.Scheduler scheduler_)
        {
            maxAmount = maxAmount_;
            duration = duration_;
            scheduler = scheduler_;
        }

        override public void Add(uint amount_)
        {
            if (amount_ == 0)
            {
                return;
            }

            var evolution = new Evolution() { Previous = Amount(), Added = amount_ };
            amount += amount_;
            if (maxAmount() > 0 && Amount() > maxAmount())
            {
                evolution.Removed = Amount() - maxAmount();
                amount = maxAmount();
                expiration = scheduler.Now + duration() * Amount();
                ScheduleRefresh();
            }
            evolution.Current = Amount();
            if (evolution.Previous == 0 || evolution.Removed > 0)
            {
                expiration = scheduler.Now + duration() * Amount();
                ScheduleRefresh();
            }
            else
            {
                expiration += duration() * (evolution.Added - evolution.Removed);
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
            if (amount <= amount_)
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
                expiration -= duration() * evolution.Removed;
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
            coroutine = scheduler.WaitUntil(expiration - (Amount() - 1) * duration());
            scheduler.StartCoroutine(coroutine);
            coroutine = null;

            var evolution = new Evolution() { Previous = Amount() };
            if (Amount() == 1)
            {
                return;
            }
            else if (Amount() == 1)
            {
                evolution.Removed = 1;
                amount = 0;
                expiration = 0;
            }
            else if (Amount() > 1)
            {
                evolution.Removed = 1;
                amount -= 1;
                expiration = scheduler.Now + duration() * Amount();
                ScheduleRefresh();
            }
            evolution.Current = amount;
            _Changed(evolution);
        }
    }
}
