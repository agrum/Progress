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
        public Numeric Left { get; private set; }
        public Numeric Right { get; private set; }
        public ERule Rule { get; private set; }

        public enum ERule
        {
            LessThan,
            LessOrEqualTo,
            EqualTo,
            GreaterOrEqualTo,
            GreaterThan
        }

        public static implicit operator Condition(JSONArray jArray_)
        {
            Condition triggerCondition = new Condition();
            if (jArray_.Count != 3)
            {
                throw new Exception();
            }
            triggerCondition.Left = jArray_[0];
            ERule rule;
            if (!Enum.TryParse(jArray_[1].ToString(), true, out rule))
                throw new NotSupportedException();
            triggerCondition.Rule = rule;
            triggerCondition.Right = jArray_[2];

            return triggerCondition;
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
