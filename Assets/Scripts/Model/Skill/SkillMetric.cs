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
}
