using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill
{
    public class Condition
    {
        public SkillMetricReference Left { get; private set; }
        public SkillMetricReference Right { get; private set; }
        public ERule Rule { get; private set; }

        public enum ERule
        {
            LessThan,
            LessOrEqualTo,
            EqualTo,
            GreaterOrEqualTo,
            GreaterThan
        }

        public Condition(SkillMetricReference left_, ERule rule_, SkillMetricReference right_)
        {
            Left = left_;
            Rule = rule_;
            Right = right_;
        }

        public static implicit operator Condition(JSONArray jArray_)
        {
            if (jArray_.Count != 3)
                throw new Exception();
            ERule rule;
            if (!Enum.TryParse(jArray_[1].ToString(), true, out rule))
                throw new NotSupportedException();

            return new Condition(jArray_[0], rule, jArray_[2]);
        }

        public static implicit operator JSONArray(Condition triggerCondition_)
        {
            JSONArray jArray = new JSONArray();

            jArray.Add(triggerCondition_.Left);
            jArray.Add(triggerCondition_.Rule.ToString("G"));
            jArray.Add(triggerCondition_.Right);

            return jArray;
        }
    }
}
