using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class Converter
    {
        public class Output
        {
            public SkillMetricReference Numeric { get; private set; }
            public ESubject Subject { get; private set; }
            public UnitGauge.ECategory Category { get; private set; }
            public NamedHash Gauge { get; private set; }

            public Output(JSONNode jNode_)
            {
                Numeric = jNode_["Numeric"];
                Subject = (ESubject)Enum.Parse(typeof(ESubject), jNode_["Subject"]);
                Category = (UnitGauge.ECategory)Enum.Parse(typeof(UnitGauge.ECategory), jNode_["Category"]);
                Gauge = jNode_["Gauge"];
            }
        }

        public ESubject As { get; private set; }
        public UnitGauge.ECategory CategoryMask { get; private set; }
        public Condition Condition { get; private set; }
        public int Order { get; private set; }
        public SkillMetricReference Input { get; private set; }
        public List<Output> Outputs { get; private set; }

        public Converter(JSONNode jNode_)
        {
            As = (ESubject)Enum.Parse(typeof(ESubject), jNode_["As"]);
            CategoryMask = (UnitGauge.ECategory)Enum.Parse(typeof(UnitGauge.ECategory), jNode_["CategoryMask"]);
            Condition = jNode_["Condition"];
            Order = jNode_["Order"].AsInt;
            Input = jNode_["input"];
        }
    }
}
