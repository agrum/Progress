using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Context.Skill.Stacker
{
    class Independent : Base
    {
        public class Grouped
        {
            public double expiration = 0;
            public uint amount = 0;
            public TaskCompletionSource<object> interrupt = new TaskCompletionSource<object>();
        }

        private MaxAmountDelegate maxAmount;
        private DurationDelegate duration;
        Utility.Scheduler scheduler;
        private uint amount = 0;
        private List<Grouped> groups = new List<Grouped>();

        public Independent(MaxAmountDelegate maxAmount_, DurationDelegate duration_, Utility.Scheduler scheduler_)
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

            var evolution = new Evolution() { Previous = amount, Added = amount_ };
            amount += amount_;
            groups.Add(new Grouped() { expiration = scheduler.Time + duration(), amount = amount_ });
            ScheduleRefresh();
            if (maxAmount() > 0 && amount > maxAmount())
            {
                evolution.Removed = amount - maxAmount();
                amount = maxAmount();
                RemoveFromGroups(evolution.Removed);
            }
            evolution.Current = amount;
            _Changed(evolution);
        }

        override public uint Amount()
        {
            return amount;
        }

        override public double Expiration()
        {
            return groups?.Last().expiration ?? 0;
        }

        override public void Remove(uint amount_)
        {
            if (amount == 0 || amount_ == 0)
            {
                return;
            }

            var evolution = new Evolution() { Previous = amount, Removed = Math.Min(amount_, amount) };
            RemoveFromGroups(evolution.Removed);
            evolution.Current = amount;
            _Changed(evolution);
        }

        void RemoveFromGroups(uint amount_)
        {
            amount_ = Math.Min(amount, amount_);
            while (groups.Count > 0 && amount_ > 0)
            {
                amount -= amount_;
                if (groups[0].amount > amount_)
                {
                    groups[0].amount -= amount_;
                    amount_ = 0;
                }
                else
                {
                    amount_ -= groups[0].amount;
                    groups[0].interrupt.TrySetCanceled();
                    groups.RemoveAt(0);
                }
            }
        }

        async void ScheduleRefresh()
        {
            var grouped = groups.Last();
            var completedTask = await Task.WhenAny(scheduler.WaitUntil(grouped.expiration), grouped.interrupt.Task);
            if (completedTask == grouped.interrupt.Task)
            {
                return;
            }
            
            if (groups.Count == 0 || groups.First() != grouped)
            {
                return;
            }

            var evolution = new Evolution() { Previous = amount, Removed = groups.First().amount };
            groups.RemoveAt(0);
            amount -= evolution.Removed;
            evolution.Current = amount;
            _Changed(evolution);
        }
    }
}
