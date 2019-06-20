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

        public MetricReference(string name_)
        {
            Name = name_;
            Metric = Skill.Reference.GetMetric(Name);
        }

        public MetricReference(double jNode_)
        {
            Value = jNode_;
        }

        public static implicit operator MetricReference(JSONNode jNode_)
        {
            if (jNode_.IsNumber)
                return new MetricReference(jNode_.AsDouble);
            else
                return new MetricReference(jNode_.Value);
        }

        public static implicit operator MetricReference(NamedHash name_)
        {
            return new MetricReference(name_.String);
        }

        public static implicit operator MetricReference(string name_)
        {
            return new MetricReference(name_);
        }

        public static implicit operator MetricReference(double value_)
        {
            return new MetricReference(value_);
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
