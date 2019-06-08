using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill
{
    public partial class MetricReference
    {
        public NamedHash Name { get; private set; }
        public Metric Metric { get; private set; }
        public double Value { get; private set; }

        public MetricReference(JSONNode jNode_)
        {
            if (jNode_.IsString)
            {
                Name = jNode_;
                Metric = Skill.Reference.GetMetric(Name);
            }
            else if (jNode_.IsNumber)
                Value = jNode_;
            else
                throw new NotSupportedException();
        }

        public static implicit operator MetricReference(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator MetricReference(NamedHash name_)
        {
            return name_;
        }

        public static implicit operator MetricReference(double value)
        {
            return value;
        }

        public static implicit operator JSONNode(MetricReference object_)
        {
            if (object_.Name != null)
                return object_.Name;
            else
                return object_.Value;
        }
    }
}
