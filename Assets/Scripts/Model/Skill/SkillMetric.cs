using System;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class SkillMetric
    {
        public string Name { get { return Json["name"]; } private set { Json["name"] = value; } }
        public string Category { get { return Json["category"]; } private set { Json["category"] = value; } }
        public float Value { get { return Json["value"]; } private set { Json["value"] = value; } }
        public int UpgType { get { return Json["upgType"]; } private set { Json["upgType"] = value; } }
        public float UpgCost { get { return Json["upgCost"]; } private set { Json["upgCost"] = value; } }

        public JSONObject Json { get; private set; }

        public SkillMetric(JSONObject json_)
        {
            Json = json_;
        }

        static public string Hash(string category_, string name_)
        {
            return category_ + "_" + name_;
        }
    }
}
