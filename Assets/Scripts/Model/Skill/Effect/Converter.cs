using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class Converter : Base
    {
        public class Output
        {
            public MetricReference Numeric { get; private set; }
            public ESubject Subject { get; private set; }
            public Gauge.ECategory Category { get; private set; }
            public NamedHash Gauge { get; private set; }

            public Output(
                MetricReference numeric_,
                ESubject subject_,
                Gauge.ECategory category_,
                NamedHash gauge_)
            {
                Numeric = numeric_;
                Subject = subject_;
                Category = category_;
                Gauge = gauge_;
            }

            public Output(JSONNode jNode_)
            {
                Numeric = jNode_["Numeric"];
                Subject = (ESubject)Enum.Parse(typeof(ESubject), jNode_["Subject"]);
                Category = (Gauge.ECategory)Enum.Parse(typeof(Gauge.ECategory), jNode_["Category"]);
                Gauge = jNode_["Gauge"];
            }

            public static implicit operator JSONNode(Output object_)
            {
                JSONObject jObject = new JSONObject();
                jObject["Numeric"] = object_.Numeric;
                jObject["Subject"] = object_.Subject.ToString("G");
                jObject["Category"] = object_.Category.ToString("G");
                jObject["Gauge"] = object_.Gauge;
                return jObject;
            }
        }

        public ESubject As { get; private set; }
        public Gauge.ECategory CategoryMask { get; private set; }
        public Condition Condition { get; private set; }
        public int Order { get; private set; }
        public MetricReference Input { get; private set; }
        public List<Output> Outputs { get; private set; } = new List<Output>();

        public Converter(
            Id id_,
            ESubject as_,
            Gauge.ECategory categorymask_,
            Condition condition_,
            int order_,
            MetricReference input_,
            params Output[] outputs_)
            : base(id_)
        {
            As = as_;
            CategoryMask = categorymask_;
            Condition = condition_;
            Order = order_;
            Input = input_;
            Outputs = new List<Output>(outputs_);
        }

        public Converter(JSONNode jNode_)
            : base(jNode_)
        {
            As = (ESubject)Enum.Parse(typeof(ESubject), jNode_["As"]);
            CategoryMask = (Gauge.ECategory)Enum.Parse(typeof(Gauge.ECategory), jNode_["CategoryMask"]);
            Condition = jNode_["Condition"];
            Order = jNode_["Order"].AsInt;
            Input = jNode_["input"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["CategoryMask"] = CategoryMask.ToString("G");
            jObject["Condition"] = Condition;
            jObject["Order"] = Order;
            jObject["Input"] = Input;
            JSONArray outputs = new JSONArray();
            foreach (var output in Outputs)
                outputs.Add(output);
            jObject["Outputs"] = outputs;
            return jObject;
        }
    }
}
