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
            //NoRefresh
            var noRefresh = new NoRefresh(() => 3, () => 0.5, scheduler);
            noRefresh.Changed += (Evolution evolution) => { Debug.Log($"NoRefresh Evolution {evolution.ToString()}"); };
            for (int i = 0; i < 100; ++i)
            {
                if (UnityEngine.Random.value < 0.35)
                {
                    uint amount = 1 + Convert.ToUInt32(UnityEngine.Random.value * 2);
                    Debug.Log($"NoRefresh Add {amount} to {noRefresh.ToString()}");
                    noRefresh.Add(amount);
                }
                else if (UnityEngine.Random.value < 0.50)
                {
                    uint amount = 1 + Convert.ToUInt32(UnityEngine.Random.value * 2);
                    Debug.Log($"NoRefresh Remove {amount} to {noRefresh.ToString()}");
                    noRefresh.Remove(amount);
                }
                yield return scheduler.Wait(0.1);
            }
        }
    }
}
