using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model
{
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
        private Modifier modifier;
        private SkillMetric skillMetric;
        private MetricUpgrade metricUpgrade;
        private Utility.Cache<float> upgradedSkillValue;
        private List<object> referencePath;

        public NumericValueSkill(string idName, Modifier modifier_)
        {
            modifier = modifier_;

            var sourceSkill = modifier.SourceSkill;
            skillMetric = sourceSkill.Metric(idName);

            var activeChampion = App.Content.Account.ActiveChampion;
            if (activeChampion != null)
                metricUpgrade = activeChampion.Upgrades[sourceSkill][skillMetric];

            upgradedSkillValue = new Utility.Cache<float>(RefreshCache, 60);

            Modifier.BuildReferencePath(skillMetric.Reference, 0, ref referencePath);
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
