using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Route
{
    public class Unit
    {

        public enum EAttribute
        {
            Life,
            Health,
            Shield,
            Armor,
            Stamina,
            MaximumLife,
            MaximumHealth,
            MaximumShield,
            MaximumArmor,
            MaximumStamina,
            LifeFraction,
            HealthFraction,
            ShieldFraction,
            ArmorFraction,
            StaminaFraction,
            Speed,
            Vision,
            Size,
            DamageMitigation,
            AilmentMitigation,
            AttackRate,
            AttackRange,
            AttackDamage,
            StaminaRecoveryRate,
            StaminaRecoveryWait,
            Distance,
            IsActive,
            IsSelf,
            IsAlly,
            IsPlayer
        }

        private Dictionary<EAttribute, Generic.RouteDelegate> ReferencePathDictionary = new Dictionary<EAttribute, Generic.RouteDelegate>();

        public Unit()
        {
            ReferencePathDictionary.Add(EAttribute.Life, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.Health, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.Shield, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.Armor, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.Stamina, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.MaximumLife, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.MaximumHealth, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.MaximumShield, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.MaximumArmor, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.MaximumStamina, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.LifeFraction, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.HealthFraction, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.ShieldFraction, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.ArmorFraction, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.StaminaFraction, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.Speed, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.Vision, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.Size, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.DamageMitigation, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.AilmentMitigation, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.AttackRate, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.AttackRange, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.AttackDamage, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.StaminaRecoveryRate, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.StaminaRecoveryWait, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.Distance, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.IsActive, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.IsSelf, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.IsAlly, ReferenceUnit);
            ReferencePathDictionary.Add(EAttribute.IsPlayer, ReferenceUnit);
        }
        //GENERIC
        public object Reference(object object_, System.Collections.IEnumerator enumerator_)
        {
            EAttribute? attribute = enumerator_.Current as EAttribute?;
            if (!attribute.HasValue)
                throw new Exception();
            return ReferencePathDictionary[attribute.Value](object_, enumerator_);
        }

        public object Life(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.Life;
        }

        public object Health(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.Health;
        }

        public object Shield(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.Shield;
        }

        public object Armor(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.Armor;
        }

        public object Stamina(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.Stamina;
        }

        public object MaximumLife(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.MaximumLife;
        }

        public object MaximumHealth(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.MaximumHealth;
        }

        public object MaximumShield(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.MaximumShield;
        }

        public object MaximumArmor(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.MaximumArmor;
        }

        public object MaximumStamina(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.MaximumStamina;
        }

        public object LifeFraction(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.LifeFraction;
        }

        public object HealthFraction(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.HealthFraction;
        }

        public object ShieldFraction(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.ShieldFraction;
        }

        public object ArmorFraction(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.ArmorFraction;
        }

        public object StaminaFraction(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.StaminaFraction;
        }

        public object Speed(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.Speed;
        }

        public object Vision(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.Vision;
        }

        public object Size(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.Size;
        }

        public object DamageMitigation(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.DamageMitigation;
        }

        public object AilmentMitigation(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.AilmentMitigation;
        }

        public object AttackRate(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.AttackRate;
        }

        public object AttackRange(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.AttackRange;
        }

        public object AttackDamage(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.AttackDamage;
        }

        public object StaminaRecoveryRate(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.StaminaRecoveryRate;
        }

        public object StaminaRecoveryWait(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.StaminaRecoveryWait;
        }

        public object Distance(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.Distance;
        }

        public object IsActive(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.IsActive;
        }

        public object IsSelf(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.IsSelf;
        }

        public object IsAlly(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.IsAlly;
        }

        public object IsPlayer(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Unit unit = object_ as Model.Unit;
            GetNextAttribute(enumerator_, false);
            return unit.IsPlayer;
        }

        private System.Collections.IEnumerator GetNextAttribute(System.Collections.IEnumerator enumerator_, bool needNext_)
        {
            if (enumerator_.MoveNext() == needNext_)
                throw new Exception();
            return enumerator_;
        }
    }
}