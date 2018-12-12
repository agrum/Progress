using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model
{
    public class ModfierReferenceBuilder
    {
        enum EAttribute
        {
            //Nested
            Modifier,
            Trigger,
            Unit,
            //Modifer
            Removable,
            Alignment,
            AmountStack,
            MaxStack,
            DetectionRadius,
            Duration,
            DurationLeft,
            TickPeriod,
            IsTicking,
            //Unit
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

        public Modifier referenceModfier;
        public Trigger referenceTrigger;
        public Unit referenceUnit;
        private Dictionary<EAttribute, NumericValueSkill.ReferencePath> ReferencePathDictionary = new Dictionary<EAttribute, NumericValueSkill.ReferencePath>();

        ModfierReferenceBuilder()
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

            ReferencePathDictionary.Add(EAttribute.Life, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.Health, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.Shield, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.Armor, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.Stamina, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.MaximumLife, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.MaximumHealth, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.MaximumShield, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.MaximumArmor, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.MaximumStamina, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.LifeFraction, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.HealthFraction, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.ShieldFraction, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.ArmorFraction, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.StaminaFraction, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.Speed, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.Vision, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.Size, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.DamageMitigation, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.AilmentMitigation, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.AttackRate, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.AttackRange, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.AttackDamage, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.StaminaRecoveryRate, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.StaminaRecoveryWait, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.Distance, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.IsActive, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.IsSelf, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.IsAlly, ReferenceIsTicking);
            ReferencePathDictionary.Add(EAttribute.IsPlayer, ReferenceIsTicking);
        }

        //GENERIC
        public object Reference(System.Collections.IEnumerator enumerator_)
        {
            EAttribute? attribute = enumerator_.Current as EAttribute?;
            if (!attribute.HasValue)
                throw new Exception();
            return ReferencePathDictionary[attribute.Value](enumerator_);
        }

        public System.Collections.IEnumerator GetNextAttribute(System.Collections.IEnumerator enumerator_, bool needNext_)
        {
            if (enumerator_.MoveNext() == needNext_)
                throw new Exception();
            return enumerator_;
        }

        //NESTING
        public object ReferenceParent(System.Collections.IEnumerator enumerator_)
        {
            referenceModfier = referenceModfier.ParentModifier;
            return Reference(GetNextAttribute(enumerator_, true));
        }

        public object ReferenceTrigger(System.Collections.IEnumerator enumerator_)
        {
            referenceTrigger = referenceModfier.CurrentTrigger;
            return Reference(GetNextAttribute(enumerator_, true));
        }

        public object ReferenceUnit(System.Collections.IEnumerator enumerator_)
        {
            referenceUnit = referenceModfier.AttachedUnit;
            return Reference(GetNextAttribute(enumerator_, true));
        }

        //MODIFIER
        public object ReferenceRemovable(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceModfier.Removable;
        }

        public object ReferenceAlignment(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceModfier.Alignment;
        }

        public object ReferenceAmountStack(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceModfier.AmountStack;
        }

        public object ReferenceMaxStack(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceModfier.MaxStack;
        }

        public object ReferenceDetectionRadius(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceModfier.DetectionRadius;
        }

        public object ReferenceDuration(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceModfier.Duration;
        }

        public object ReferenceDurationLeft(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceModfier.DurationLeft;
        }

        public object ReferenceTickPeriod(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceModfier.TickPeriod;
        }

        public object ReferenceIsTicking(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceModfier.IsTicking;
        }

        //UNIT
        public object ReferenceLife(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.Life;
        }

        public object ReferenceHealth(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.Health;
        }

        public object ReferenceShield(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.Shield;
        }

        public object ReferenceArmor(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.Armor;
        }

        public object ReferenceStamina(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.Stamina;
        }

        public object ReferenceMaximumLife(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.MaximumLife;
        }

        public object ReferenceMaximumHealth(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.MaximumHealth;
        }

        public object ReferenceMaximumShield(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.MaximumShield;
        }

        public object ReferenceMaximumArmor(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.MaximumArmor;
        }

        public object ReferenceMaximumStamina(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.MaximumStamina;
        }

        public object ReferenceLifeFraction(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.LifeFraction;
        }

        public object ReferenceHealthFraction(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.HealthFraction;
        }

        public object ReferenceShieldFraction(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.ShieldFraction;
        }

        public object ReferenceArmorFraction(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.ArmorFraction;
        }

        public object ReferenceStaminaFraction(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.StaminaFraction;
        }

        public object ReferenceSpeed(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.Speed;
        }

        public object ReferenceVision(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.Vision;
        }

        public object ReferenceSize(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.Size;
        }

        public object ReferenceDamageMitigation(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.DamageMitigation;
        }

        public object ReferenceAilmentMitigation(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.AilmentMitigation;
        }

        public object ReferenceAttackRate(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.AttackRate;
        }

        public object ReferenceAttackRange(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.AttackRange;
        }

        public object ReferenceAttackDamage(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.AttackDamage;
        }

        public object ReferenceStaminaRecoveryRate(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.StaminaRecoveryRate;
        }

        public object ReferenceStaminaRecoveryWait(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.StaminaRecoveryWait;
        }

        public object ReferenceDistance(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.Distance;
        }

        public object ReferenceIsActive(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.IsActive;
        }

        public object ReferenceIsSelf(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.IsSelf;
        }

        public object ReferenceIsAlly(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.IsAlly;
        }

        public object ReferenceIsPlayer(System.Collections.IEnumerator enumerator_)
        {
            GetNextAttribute(enumerator_, false);
            return referenceUnit.IsPlayer;
        }
    }

    public abstract class INumericValue
    {
        public static implicit operator float(INumericValue numericValue)
        {
            return numericValue.Get();
        }

        public static implicit operator string(INumericValue numericValue)
        {
            return numericValue.Literal();
        }

        public static INumericValue Make(JSONNode node_, Modifier modifier_)
        {
            if (node_.IsNumber)
                return new NumericValueFlat(node_.AsFloat);
            else if(node_.IsString)
                return new NumericValueSkill(node_, modifier_);
            throw new Exception();
        }

        protected abstract float Get();
        protected abstract string Literal();
    }

    public class NumericValueFlat : INumericValue
    {
        private float value;

        public NumericValueFlat(float value_)
        {
            value = value_;
        }

        protected override float Get()
        {
            return value;
        }

        protected override string Literal()
        {
            return value.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class NumericValueSkill : INumericValue
    {
        public delegate object ReferencePath(System.Collections.IEnumerator enumerator_);

        private Modifier modifier;
        private SkillMetric skillMetric;
        private MetricUpgrade metricUpgrade;
        private Utility.Cache<float> upgradedSkillValue;
        private List<NumericValueSkill.ReferencePath> referencePath;

        public NumericValueSkill(string idName, Modifier modifier_)
        {
            modifier = modifier_;

            var sourceSkill = modifier.SourceSkill;
            skillMetric = sourceSkill.Metric(idName);

            var activeChampion = App.Content.Account.ActiveChampion;
            if (activeChampion != null)
                metricUpgrade = activeChampion.Upgrades[sourceSkill][skillMetric];

            upgradedSkillValue = new Utility.Cache<float>(RefreshCache, 60);

            var stringReference = skillMetric.Reference;
            referencePath = new List<ReferencePath>();
            Modifier.BuildReferencePath(stringReference.GetEnumerator(), ref referencePath);
        }

        protected override float Get()
        {
            return upgradedSkillValue;
        }

        protected override string Literal()
        {
            float value = upgradedSkillValue;
            string sValue;
            if (skillMetric.ValueType == SkillMetric.EValueType.Add)
                sValue = value.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            else
                sValue = value.ToString("P1", System.Globalization.CultureInfo.InvariantCulture) + "%";
            return sValue;
        }

        private float RefreshCache()
        {
            float value = skillMetric.Value;
            if (metricUpgrade != null)
                value *= metricUpgrade.Factor();

            return value;
        }
    }
}
