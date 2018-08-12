using System;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Model
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
        public float Value { get { return Json["value"]; } }
        public EValueType ValueType { get { return Json["value"] == "Add" ? EValueType.Add : EValueType.Mult; } }
        public string[] Reference { get { return Json["reference"].Value.ToString().Split('.'); } }
        public int UpgType { get { return Json["upgType"]; } }
        public float UpgCost { get { return Json["upgCost"]; } }

        public JSONObject Json { get; private set; }

        public SkillMetric(JSONObject json_)
        {
            Json = json_;
        }
    }
}
