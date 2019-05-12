using System;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Model.Skill
{
    public class SkillMetric
    {
        public enum EValueType
        {
            Add,
            Mult
        }

        public string IdName { get { return Json["idName"]; } }
        public string Name { get { return Json["name"]; } }
        public string Category { get { return Json["category"]; } }
        public double Value { get { return Json["value"]; } }
        public EValueType ValueType { get { EValueType valueType; Enum.TryParse(Json["valueType"], true, out valueType); return valueType; } }
        public Numeric Numeric { get { return Json["numeric"]; } }
        public int UpgType { get { return Json["upgType"]; } }
        public float UpgCost { get { return Json["upgCost"]; } }

        public JSONObject Json { get; private set; }

        public SkillMetric(JSONObject json_)
        {
            Json = json_;
        }

        public static implicit operator JSONNode(SkillMetric numeric_)
        {
            return numeric_.Json;
        }
    }

    public class SkillMetricReference
    {
        string NumericId;
        Numeric Numeric;
        double Value;

        public SkillMetricReference(JSONNode jNode_)
        {
            if (jNode_.IsString)
            {
                NumericId = jNode_;
                Numeric = jNode_; //TODO, fetch numeric from skill metric id name
            }
            else if (jNode_.IsNumber)
                Value = jNode_;
            else
                throw new NotSupportedException();
        }

        public static implicit operator SkillMetricReference(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator SkillMetricReference(string numericId_)
        {
            return numericId_;
        }

        public static implicit operator SkillMetricReference(double value)
        {
            return value;
        }

        public static implicit operator JSONNode(SkillMetricReference object_)
        {
            if (object_.NumericId.Length > 0)
                return object_.NumericId;
            else
                return object_.Value;
        }

        public double Get(TriggerInfo triggerInfo_)
        {
            if (Numeric == null)
            {
                if (NumericId.Length > 0)
                    Numeric = null;  //TODO
                else
                    Numeric = Value.ToString();
            }

            return Numeric.Get(triggerInfo_);
        }
    }
}
