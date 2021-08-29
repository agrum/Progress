﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill
{
    public class Condition
    {
        public MetricReference Left { get; private set; }
        public MetricReference Right { get; private set; }
        public ERule Rule { get; private set; }

        public enum ERule
        {
            LessThan,
            LessOrEqualTo,
            EqualTo,
            GreaterOrEqualTo,
            GreaterThan
        }

        public Condition(MetricReference left_, ERule rule_, MetricReference right_)
        {
            Left = left_;
            Rule = rule_;
            Right = right_;
        }

        public static implicit operator Condition(JSONNode jNode)
        {
            if (!jNode.IsArray)
                throw new Exception();
            var jArray = jNode.AsArray;
            if (jArray.Count != 3)
                throw new Exception();
            ERule rule = Serializer.ReadEnum<ERule>(jArray[1]);

            return new Condition(jArray[0], rule, jArray[2]);
        }

        public static implicit operator JSONNode(Condition triggerCondition_)
        {
            JSONArray jArray = new JSONArray();

            jArray.Add(triggerCondition_.Left);
            jArray.Add(Serializer.WriteEnum(triggerCondition_.Rule));
            jArray.Add(triggerCondition_.Right);

            return jArray;
        }
    }
}