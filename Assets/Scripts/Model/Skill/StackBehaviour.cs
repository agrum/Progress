using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill
{
    class StackBehaviour
    {
        public enum EStackPolicy
        {
            FlatOne,
            Linear,
        }

        public enum ERefreshPolicy
        {
            Instant,
            NoRefresh,
            IndependentStack,
            FullRefresh,
            Cumulative,
            Permanent,
        }

        public EStackPolicy StackPolicy { get; private set; }
        public ERefreshPolicy RefreshPolicy { get; private set; }
        public uint MaxStack { get; private set; }
        public double Duration { get; private set; }
        public uint Amount { get; private set; } = 0;
        private double Lifetime = 0;
        private List<Tuple<double, uint>> Stacks = new List<Tuple<double, uint>>();

        public StackBehaviour()
        {
            StackPolicy = EStackPolicy.Linear;
            RefreshPolicy = ERefreshPolicy.Instant;
            MaxStack = uint.MaxValue;
            Duration = 0.0;
        }

        public StackBehaviour(EStackPolicy stackPolicy_, ERefreshPolicy refreshPolicy_, uint maxStack_, double duration_ = 0.0)
        {
            StackPolicy = stackPolicy_;
            RefreshPolicy = refreshPolicy_;
            MaxStack = maxStack_;
            Duration = duration_;
            if (RefreshPolicy == ERefreshPolicy.Instant || RefreshPolicy == ERefreshPolicy.Permanent)
                Duration = 0;
        }

        public StackBehaviour(JSONNode jNode_)
        {
            StackPolicy = (EStackPolicy)jNode_["stackPolicy"].AsInt;
            RefreshPolicy = (ERefreshPolicy)jNode_["refreshPolicy"].AsInt;
            MaxStack = (uint)jNode_["maxStack"].AsInt;
            Duration = jNode_["duration"].AsDouble;
        }

        public static implicit operator StackBehaviour(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator JSONNode(StackBehaviour numeric_)
        {
            JSONObject jObject = new JSONObject();
            jObject["stackPolicy"] = (int)numeric_.StackPolicy;
            jObject["refreshPolicy"] = (int)numeric_.RefreshPolicy;
            jObject["maxStack"] = (int)numeric_.MaxStack;
            jObject["duration"] = numeric_.Duration;
            return jObject;
        }

        public void Add(uint delta_)
        {
            Amount += delta_;
            switch (RefreshPolicy)
            {
                case ERefreshPolicy.Instant:
                case ERefreshPolicy.Permanent:
                    break;
                case ERefreshPolicy.NoRefresh:
                    if (Amount == 0)
                        Stacks.Add(new Tuple<double, uint>(Duration + Lifetime, delta_));
                    else
                        Stacks[0] = new Tuple<double, uint>(Stacks[0].Item1, Stacks[0].Item2 + delta_);
                    break;
                case ERefreshPolicy.IndependentStack:
                    Stacks.Add(new Tuple<double, uint>(Duration + Lifetime, delta_));
                    break;
                case ERefreshPolicy.FullRefresh:
                case ERefreshPolicy.Cumulative:
                    if (Amount == 0)
                        Stacks.Add(new Tuple<double, uint>(Duration + Lifetime, delta_));
                    else
                        Stacks[0] = new Tuple<double, uint>(Duration + Lifetime, Stacks[0].Item2 + delta_);
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
            
            if (RefreshPolicy == ERefreshPolicy.Cumulative)
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
            if (RefreshPolicy == ERefreshPolicy.Instant || RefreshPolicy == ERefreshPolicy.Permanent)
                    return;

            Lifetime += dt_;
            if (Amount == 0)
                return;

            if (RefreshPolicy == ERefreshPolicy.Cumulative && Amount > 0)
            {
                uint amount = (uint) Convert.ToInt32(0.49 + (Stacks[0].Item1 - Lifetime) / Duration);
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
