using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
{
    public class Converter : Base
    {
        public class Output
        {
            public MetricReference Numeric { get; private set; }
            public ESubject Subject { get; private set; }
            public Data.Skill.Unit.Stat.ECategory Category { get; private set; }
            public NamedHash Gauge { get; private set; }

            public Output(
                MetricReference numeric_,
                ESubject subject_,
                Data.Skill.Unit.Stat.ECategory category_,
                NamedHash gauge_)
            {
                Numeric = numeric_;
                Subject = subject_;
                Category = category_;
                Gauge = gauge_;
            }

            public Output(JSONNode jNode_)
            {
                Numeric = jNode_["numeric"];
                Subject = Serializer.ReadEnum<ESubject>(jNode_["subject"]);
                Category = Serializer.ReadEnum<Data.Skill.Unit.Stat.ECategory>(jNode_["category"]);
                Gauge = jNode_["gauge"];
            }

            public static implicit operator JSONNode(Output object_)
            {
                JSONObject jObject = new JSONObject();
                jObject["numeric"] = object_.Numeric;
                jObject["subject"] = object_.Subject;
                jObject["category"] = object_.Category;
                jObject["gauge"] = object_.Gauge;
                return jObject;
            }
        }

        public ESubject As { get; private set; }
        public Data.Skill.Unit.Stat.ECategory CategoryMask { get; private set; }
        public Condition Condition { get; private set; }
        public int Order { get; private set; }
        public MetricReference Input { get; private set; }
        public List<Output> Outputs { get; private set; } = new List<Output>();

        public Converter(
            Id id_,
            ESubject as_,
            Data.Skill.Unit.Stat.ECategory categorymask_,
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
            As = Serializer.ReadEnum<ESubject>(jNode_["as"]);
            CategoryMask = Serializer.ReadEnum<Data.Skill.Unit.Stat.ECategory>(jNode_["categoryMask"]);
            Condition = jNode_["condition"];
            Order = jNode_["order"].AsInt;
            Input = jNode_["input"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["categoryMask"] = CategoryMask;
            jObject["condition"] = Condition;
            jObject["order"] = Order;
            jObject["input"] = Input;
            JSONArray outputs = new JSONArray();
            foreach (var output in Outputs)
                outputs.Add(output);
            jObject["outputs"] = outputs;
            return jObject;
        }
    }
}
