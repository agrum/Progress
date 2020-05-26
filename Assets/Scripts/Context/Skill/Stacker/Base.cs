using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Context.Skill.Stacker
{
    public abstract class Base
    {
        public class Evolution
        {
            public uint Previous = 0;
            public uint Current = 0;
            public uint Added = 0;
            public uint Removed = 0;

            public override string ToString()
            {
                return $"({Previous} -> {Current} +{Added} -{Removed})";
            }
        }

        public abstract uint Amount();
        public abstract double Expiration();
        public abstract void Add(uint amount_);
        public abstract void Remove(uint amount_);

        public double Duration(Utility.Scheduler scheduler)
        {
            return Amount() > 0 ? Expiration() - scheduler.Now : 0;
        }

        protected void _Changed(Evolution evolution_)
        {
            Changed(evolution_);
        }

        public override string ToString()
        {
            return $"({Amount()} until {Expiration().ToString("F1")}s)";
        }

        public delegate double DurationDelegate();
        public delegate uint MaxAmountDelegate();

        public delegate void EvolutionDelegate(Evolution evolution_);
        public event EvolutionDelegate Changed = delegate { };

        static public IEnumerator UnitTest(Utility.Scheduler scheduler)
        {
            List<Base> stackers = new List<Base>();
            stackers.Add(new NoRefresh(() => 3, () => 0.5, scheduler));
            stackers.Add(new Refresh(() => 3, () => 0.5, scheduler));
            stackers.Add(new Independent(() => 3, () => 0.5, scheduler));
            stackers.Add(new Cumulative(() => 3, () => 0.5, scheduler));
            stackers.Add(new Permanent(() => 3));

            foreach (var stacker in stackers)
            {
                stacker.Changed += (Evolution evolution) => { Debug.Log($"{stacker.GetType().Name} Evolution {evolution.ToString()} with {stacker.Duration(scheduler).ToString("F1")}s left at {scheduler.Now.ToString("F1")}s"); };
                for (int i = 0; i < 100; ++i)
                {
                    if (UnityEngine.Random.value < 0.35)
                    {
                        uint amount = 1 + Convert.ToUInt32(UnityEngine.Random.value * 2);
                        Debug.Log($"{stacker.GetType().Name} Add {amount} to {stacker.ToString()}");
                        stacker.Add(amount);
                    }
                    else if (UnityEngine.Random.value < 0.50)
                    {
                        uint amount = 1 + Convert.ToUInt32(UnityEngine.Random.value * 2);
                        Debug.Log($"{stacker.GetType().Name} Remove {amount} to {stacker.ToString()}");
                        stacker.Remove(amount);
                    }
                    yield return scheduler.Wait(0.1);
                }
            }
        }
    }
}
