using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Context.Skill.Stacker
{
    class NoRefresh : Base
    {
        private MaxAmountDelegate maxAmount;
        private DurationDelegate duration;
        Utility.Scheduler scheduler;
        TaskCompletionSource<object> interruptTask = null;
        private uint amount = 0;
        private double expiration = 0;

        public NoRefresh(MaxAmountDelegate maxStack_, DurationDelegate duration_, Utility.Scheduler scheduler_)
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
            if (evolution.Previous == 0)
            {
                expiration = scheduler.Time + duration();
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
            if (amount == 0 || amount_ == 0)
            {
                return;
            }

            var evolution = new Evolution() { Previous = amount };
            if (amount >= amount_)
            {
                evolution.Removed = amount;
                amount = 0;
                expiration = 0;
            }
            else
            {
                evolution.Removed = amount_;
                amount -= amount_;
            }
            evolution.Current = amount;
            _Changed(evolution);
        }

        async void ScheduleRefresh()
        {
            interruptTask?.TrySetCanceled();
            var localInterruptTask = new TaskCompletionSource<object>();
            interruptTask = localInterruptTask;

            var completedTask = await Task.WhenAny(scheduler.WaitUntil(Expiration()), interruptTask.Task);
            if (completedTask == localInterruptTask.Task)
            {
                interruptTask = null;
                return;
            }

            Remove(Amount());
        }
    }
}
