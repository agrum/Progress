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
        enum EAlignment
        {
            Good,
            Neutral,
            Bad,
        }

        enum ENode
        {
            Parent,
            Trigger,
            Unit,
            Removable,
            Alignment,
            AmountStack,
            MaxStack,
            DetectionRadius,
            Duration,
            DurationLeft,
            TickPeriod
        }

        public bool ToDestroy { get; private set; } = false;

        Modifier ParentModifier;
        Trigger CurrentTrigger;
        Unit AttachedUnit;

        public Skill SourceSkill { get; private set; }
        public Modifier SourceModifier { get; private set; }
        public string IdName { get; private set; }
        bool Removable;
        EAlignment Alignment;
        int MaxStack;
        public float DetectionRadius { get; private set; } = -1;
        float Duration = -1;
        public float ExpirationTime { get; private set; }
        public float TickPeriod { get; private set; }
        public bool Ticks { get; private set; } = true;

        private List<IModifierOperator> OperatorList = new List<IModifierOperator>();
        private List<Trigger> TriggerList = new List<Trigger>();

        public Modifier(Skill sourceSkill_, Modifier sourceModifier_, Unit attachedUnit_, JSONObject json_)
        {
            SourceSkill = sourceSkill_;
            SourceModifier = sourceModifier_;
            AttachedUnit = attachedUnit_;

            if (TickPeriod > 0)
                Ticks = true;
            AttachedUnit.ApplyModifier(this);
        }

        static public void BuildReferencePath(string[] stringPath, int index_, ref List<object> referencePath)
        {
            string node = stringPath[index_];
            if (node.Equals("Parent"))
            {
                referencePath.Add(ENode.Parent);
                BuildReferencePath(stringPath, ++index_, ref referencePath);
            }
            else if (node.Equals("Trigger"))
            {
                referencePath.Add(ENode.Trigger);
                Trigger.BuildReferencePath(stringPath, ++index_, ref referencePath);
            }
            else if (node.Equals("Unit"))
            {
                referencePath.Add(ENode.Unit);
                Unit.BuildReferencePath(stringPath, ++index_, ref referencePath);
            }
        }

        public float Reference(List<object> referencePath_, int index_)
        {
            ENode? node = referencePath_[index_] as ENode?;
            if (node == null)
                throw new Exception();

            switch (node)
            {
                case ENode.Parent: return ParentModifier.Reference(referencePath_, ++index_);
                case ENode.Trigger: return CurrentTrigger.Reference(referencePath_, ++index_);
                case ENode.Unit: return AttachedUnit.Reference(referencePath_, ++index_);
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
                Trigger(trigger);
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
