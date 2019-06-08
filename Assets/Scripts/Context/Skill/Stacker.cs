using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Context.Skill
{
    public class Stacker
    {
        public Data.Skill.Stacker Dta { get; private set; }
        public uint Amount { get; private set; } = 0;
        private double Lifetime = 0;
        private List<Tuple<double, uint>> Stacks = new List<Tuple<double, uint>>();

        public Stacker(Data.Skill.Stacker data_)
        {
            Dta = data_;
        }

        public void Add(uint delta_)
        {
            Amount += delta_;
            switch (Dta.RefreshPolicy)
            {
                case Data.Skill.Stacker.ERefreshPolicy.Instant:
                case Data.Skill.Stacker.ERefreshPolicy.Permanent:
                    break;
                case Data.Skill.Stacker.ERefreshPolicy.NoRefresh:
                    if (Amount == 0)
                        Stacks.Add(new Tuple<double, uint>(Dta.Duration + Lifetime, delta_));
                    else
                        Stacks[0] = new Tuple<double, uint>(Stacks[0].Item1, Stacks[0].Item2 + delta_);
                    break;
                case Data.Skill.Stacker.ERefreshPolicy.IndependentStack:
                    Stacks.Add(new Tuple<double, uint>(Dta.Duration + Lifetime, delta_));
                    break;
                case Data.Skill.Stacker.ERefreshPolicy.FullRefresh:
                case Data.Skill.Stacker.ERefreshPolicy.Cumulative:
                    if (Amount == 0)
                        Stacks.Add(new Tuple<double, uint>(Dta.Duration + Lifetime, delta_));
                    else
                        Stacks[0] = new Tuple<double, uint>(Dta.Duration + Lifetime, Stacks[0].Item2 + delta_);
                    break;
            }
        }

        public void Remove(uint delta_)
        {
            if (delta_ >= Amount)
            {
                Amount = 0;
                Stacks.Clear();
                return;
            }

            if (Dta.RefreshPolicy == Data.Skill.Stacker.ERefreshPolicy.Cumulative)
            {
                var amount = Amount - delta_;
                Amount = 0;
                Stacks.Clear();
                Add(amount);
                return;
            }

            Amount -= delta_;
            while (delta_ > 0)
            {
                var storedAmount = Stacks[0].Item2;
                if (storedAmount > delta_)
                    Stacks[0] = new Tuple<double, uint>(Stacks[0].Item1, storedAmount - delta_);
                else
                    Stacks.RemoveAt(0);
                delta_ -= storedAmount;
            }
        }

        public void Clear()
        {
            Remove(Amount);
        }

        public void Update(double dt_)
        {
            if (Dta.RefreshPolicy == Data.Skill.Stacker.ERefreshPolicy.Instant || Dta.RefreshPolicy == Data.Skill.Stacker.ERefreshPolicy.Permanent)
                return;

            Lifetime += dt_;
            if (Amount == 0)
                return;

            if (Dta.RefreshPolicy == Data.Skill.Stacker.ERefreshPolicy.Cumulative && Amount > 0)
            {
                uint amount = (uint)Convert.ToInt32(0.49 + (Stacks[0].Item1 - Lifetime) / Dta.Duration);
                if (amount != Stacks[0].Item2)
                {
                    Amount = 0;
                    Stacks.Clear();
                    Add(amount);
                }
                return;
            }

            while (Stacks.Count > 0 && Stacks[0].Item1 <= Lifetime)
                Stacks.RemoveAt(0);
        }
    }
}
