using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Schema
{
    public class MetricUpgrade
    {
        public string Name { get { return Json["name"]; } protected set { Json["name"] = value; } }
        public string Category { get { return Json["category"]; } protected set { Json["category"] = value; } }
        public float Level { get { return Json["level"]; } protected set { Json["level"] = value; } }

        public JSONObject Json { get; protected set; } = null;

        public MetricUpgrade(JSONObject json_)
        {
            if (json_ != null)
                Json = json_;
            else
            {
                Json = new JSONObject();
                Category = "";
                Name = "";
                Level = 0;
            }
        }

        protected void Verify()
        {
            Debug.Assert(!string.Equals(Name, ""));
            Debug.Assert(!string.Equals(Category, ""));
        }
    }
}
