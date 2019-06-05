using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill
{
    public class SkillMetricReference
    {
        string NumericId;
        Numeric Numeric;
        double Value;

        public SkillMetricReference(JSONNode jNode_)
        {
            if (jNode_.IsString)
            {
                NumericId = jNode_;
                Numeric = jNode_; //TODO, fetch numeric from skill metric id name
            }
            else if (jNode_.IsNumber)
                Value = jNode_;
            else
                throw new NotSupportedException();
        }

        public static implicit operator SkillMetricReference(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator SkillMetricReference(string numericId_)
        {
            return numericId_;
        }

        public static implicit operator SkillMetricReference(double value)
        {
            return value;
        }

        public static implicit operator JSONNode(SkillMetricReference object_)
        {
            if (object_.NumericId.Length > 0)
                return object_.NumericId;
            else
                return object_.Value;
        }

        public double Get(TriggerInfo triggerInfo_)
        {
            if (Numeric == null)
            {
                if (NumericId.Length > 0)
                    Numeric = null;  //TODO
                else
                    Numeric = Value.ToString();
            }

            return Numeric.Get(triggerInfo_);
        }
    }
}
