using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Modifier
    {
        public enum EAlignment
        {
            Good,
            Neutral,
            Bad,
        }

        public bool ToDestroy { get; private set; } = false;

        public Modifier ParentModifier { get; private set; }
        public Trigger CurrentTrigger { get; private set; }
        public Unit AttachedUnit { get; private set; }

        public Skill SourceSkill { get; private set; }
        public Modifier SourceModifier { get; private set; }
        public string IdName { get; private set; }
        public bool Removable { get; private set; }
        public EAlignment Alignment { get; private set; }
        public int AmountStack { get; private set; } = 0;
        public int MaxStack { get; private set; }
        public float DetectionRadius { get; private set; } = -1;
        public float Duration { get; private set; } = -1;
        public float DurationLeft { get; private set; }
        public float TickPeriod { get; private set; }
        public bool IsTicking { get; private set; } = true;

        private List<IModifierOperator> OperatorList = new List<IModifierOperator>();
        private List<Trigger> TriggerList = new List<Trigger>();

        public Modifier(Skill sourceSkill_, Modifier sourceModifier_, Unit attachedUnit_, JSONObject json_)
        {
            SourceSkill = sourceSkill_;
            SourceModifier = sourceModifier_;
            AttachedUnit = attachedUnit_;

            if (TickPeriod > 0)
                IsTicking = true;
            AttachedUnit.ApplyModifier(this);
        }

        public object Route(System.Collections.IEnumerator enumerator_)
        {
            return App.Route.Modifier.Reference(this, enumerator_);
        }

        static public void BuildReferencePath(System.Collections.IEnumerator stringEnumerator, ref List<NumericValueSkill.ReferencePath> referencePath)
        {
            var node = stringEnumerator.Current as string;
            var hasNext = stringEnumerator.MoveNext();
            if (node.Equals("Parent"))
            {
                ReferencePathDictionary.Add(ENode.Parent, ReferenceParent);
                BuildReferencePath(stringEnumerator, ref referencePath);
            }
            else if (node.Equals("Trigger"))
            {
                referencePath.Add(ReferenceTrigger);
                Trigger.BuildReferencePath(stringEnumerator, ref referencePath);
            }
            else if (node.Equals("Unit"))
            {
                referencePath.Add(ReferenceUnit);
                Unit.BuildReferencePath(stringEnumerator, ref referencePath);
            }
            else if (node.Equals("Removable"))
                referencePath.Add(ENode.Removable);
            else if (node.Equals("Alignment"))
                referencePath.Add(ENode.Alignment);
            else if (node.Equals("AmountStack"))
                referencePath.Add(ENode.AmountStack);
            else if (node.Equals("MaxStack"))
                referencePath.Add(ENode.MaxStack);
            else if (node.Equals("DetectionRadius"))
                referencePath.Add(ENode.DetectionRadius);
            else if (node.Equals("Duration"))
                referencePath.Add(ENode.Duration);
            else if (node.Equals("DurationLeft"))
                referencePath.Add(ENode.DurationLeft);
            else if (node.Equals("TickPeriod"))
                referencePath.Add(ENode.TickPeriod);
            else if (node.Equals("IsTicking"))
                referencePath.Add(ENode.IsTicking);
            else
                throw new Exception();
        }

        public object Reference(List<object> referencePath_, int index_)
        {
            ENode? node = referencePath_[index_] as ENode?;
            if (node == null)
                throw new Exception();

            switch (node)
            {
                case ENode.Parent: return ParentModifier.Reference(referencePath_, ++index_);
                case ENode.Trigger: return CurrentTrigger.Reference(referencePath_, ++index_);
                case ENode.Unit: return AttachedUnit.Reference(referencePath_, ++index_);
                case ENode.Removable: return Removable;
                case ENode.Alignment: return Alignment;
                case ENode.AmountStack: return AmountStack;
                case ENode.MaxStack: return MaxStack;
                case ENode.DetectionRadius: return DetectionRadius;
                case ENode.Duration: return Duration;
                case ENode.DurationLeft: return DurationLeft;
                case ENode.TickPeriod: return TickPeriod;
                case ENode.IsTicking: return IsTicking;
            }

            throw new Exception();
        }

        public T GetOperator<T>() where T : class
        {
            foreach (var ope in OperatorList)
            {
                var converted = ope as T;
                if (converted != null)
                    return converted;
            }

            return default(T);
        }

        public void CreateTriggers()
        {
            foreach (var ope in OperatorList)
                ope.Update();
        }

        public void ProcessTriggers()
        {
            foreach (var trigger in TriggerList)
                ProcessTrigger(trigger);
        }

        public void Tick()
        {

        }

        public void Kill()
        {
            ToDestroy = true;
        }

        private void ProcessTrigger(Trigger trigger_)
        {
            CurrentTrigger = trigger_;
        }
    }
}
