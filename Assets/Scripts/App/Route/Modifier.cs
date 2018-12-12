using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Route
{
    public class Modifier
    {
        enum EAttribute
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
            TickPeriod,
            IsTicking,
        }

        private Dictionary<EAttribute, Generic.RouteDelegate> ReferencePathDictionary = new Dictionary<EAttribute, Generic.RouteDelegate>();

        public Modifier()
        {
            ReferencePathDictionary.Add(EAttribute.Parent, ReferenceParent);
            ReferencePathDictionary.Add(EAttribute.Trigger, ReferenceTrigger);
            ReferencePathDictionary.Add(EAttribute.Unit, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.Removable, ReferenceRemovable);
            ReferencePathDictionary.Add(EAttribute.Alignment, ReferenceAlignment);
            ReferencePathDictionary.Add(EAttribute.AmountStack, ReferenceAmountStack);
            ReferencePathDictionary.Add(EAttribute.MaxStack, ReferenceMaxStack);
            ReferencePathDictionary.Add(EAttribute.DetectionRadius, ReferenceDetectionRadius);
            ReferencePathDictionary.Add(EAttribute.Duration, ReferenceDuration);
            ReferencePathDictionary.Add(EAttribute.DurationLeft, ReferenceDurationLeft);
            ReferencePathDictionary.Add(EAttribute.DurationLeft, ReferenceDurationLeft);
            ReferencePathDictionary.Add(EAttribute.TickPeriod, ReferenceTickPeriod);
            ReferencePathDictionary.Add(EAttribute.IsTicking, ReferenceIsTicking);
        }
        //GENERIC
        public object Reference(object object_, System.Collections.IEnumerator enumerator_)
        {
            EAttribute? attribute = enumerator_.Current as EAttribute?;
            if (!attribute.HasValue)
                throw new Exception();
            return ReferencePathDictionary[attribute.Value](object_, enumerator_);
        }

        //NESTING
        public object ReferenceParent(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            return Reference(modifier.ParentModifier, GetNextAttribute(enumerator_, true));
        }

        public object ReferenceTrigger(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            return App.Route.Trigger.Reference(modifier.CurrentTrigger, GetNextAttribute(enumerator_, true));
        }

        public object ReferenceUnit(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            return App.Route.Unit.Reference(modifier.AttachedUnit, GetNextAttribute(enumerator_, true));
        }

        //MODIFIER
        public object ReferenceRemovable(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            GetNextAttribute(enumerator_, false);
            return modifier.Removable;
        }

        public object ReferenceAlignment(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            GetNextAttribute(enumerator_, false);
            return modifier.Alignment;
        }

        public object ReferenceAmountStack(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            GetNextAttribute(enumerator_, false);
            return modifier.AmountStack;
        }

        public object ReferenceMaxStack(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            GetNextAttribute(enumerator_, false);
            return modifier.MaxStack;
        }

        public object ReferenceDetectionRadius(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            GetNextAttribute(enumerator_, false);
            return modifier.DetectionRadius;
        }

        public object ReferenceDuration(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            GetNextAttribute(enumerator_, false);
            return modifier.Duration;
        }

        public object ReferenceDurationLeft(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            GetNextAttribute(enumerator_, false);
            return modifier.DurationLeft;
        }

        public object ReferenceTickPeriod(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            GetNextAttribute(enumerator_, false);
            return modifier.TickPeriod;
        }

        public object ReferenceIsTicking(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Modifier modifier = object_ as Model.Modifier;
            GetNextAttribute(enumerator_, false);
            return modifier.IsTicking;
        }
        
        private System.Collections.IEnumerator GetNextAttribute(System.Collections.IEnumerator enumerator_, bool needNext_)
        {
            if (enumerator_.MoveNext() == needNext_)
                throw new Exception();
            return enumerator_;
        }
    }
}