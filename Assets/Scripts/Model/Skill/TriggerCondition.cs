using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill
{
    public class TriggerCondition
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

        public static implicit operator TriggerCondition(JSONArray jArray_)
        {
            TriggerCondition triggerCondition = new TriggerCondition();
            if (jArray_.Count != 3)
            {
                throw new Exception();
            }
            triggerCondition.Left = jArray_[0];
            triggerCondition.Rule = (ERule) jArray_[1].AsInt;
            triggerCondition.Left = jArray_[2];

            return triggerCondition;
        }

        public static implicit operator JSONArray(TriggerCondition triggerCondition_)
        {
            JSONArray jArray = new JSONArray();

            jArray.Add(triggerCondition_.Left);
            jArray.Add((int) triggerCondition_.Rule);
            jArray.Add(triggerCondition_.Right);

            return jArray;
        }
    }
}
