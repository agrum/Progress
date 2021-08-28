using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill
{
    public class Stacker
    {
        public enum EStackPolicy
        {
            FlatOne,
            Linear,
        }

        public enum ERefreshPolicy
        {
            Instant,
            NoRefresh,
            IndependentStack,
            FullRefresh,
            Cumulative,
            Permanent,
        }

        public EStackPolicy StackPolicy { get; private set; }
        public ERefreshPolicy RefreshPolicy { get; private set; }
        public uint MaxStack { get; private set; }
        public double Duration { get; private set; }

        public Stacker()
        {
            StackPolicy = EStackPolicy.Linear;
            RefreshPolicy = ERefreshPolicy.Instant;
            MaxStack = uint.MaxValue;
            Duration = 0.0;
        }

        public Stacker(EStackPolicy stackPolicy_, ERefreshPolicy refreshPolicy_, uint maxStack_, double duration_ = 0.0)
        {
            StackPolicy = stackPolicy_;
            RefreshPolicy = refreshPolicy_;
            MaxStack = maxStack_;
            Duration = duration_;
            if (RefreshPolicy == ERefreshPolicy.Instant || RefreshPolicy == ERefreshPolicy.Permanent)
                Duration = 0;
        }

        public Stacker(JSONNode jNode_)
        {
            StackPolicy = (EStackPolicy)jNode_["stackPolicy"].AsInt;
            RefreshPolicy = (ERefreshPolicy)jNode_["refreshPolicy"].AsInt;
            MaxStack = (uint)jNode_["maxStack"].AsInt;
            Duration = jNode_["duration"].AsDouble;
        }

        public static implicit operator Stacker(JSONNode jNode_)
        {
            return new Stacker(jNode_);
        }

        public static implicit operator JSONNode(Stacker numeric_)
        {
            JSONObject jObject = new JSONObject();
            jObject["stackPolicy"] = (int)numeric_.StackPolicy;
            jObject["refreshPolicy"] = (int)numeric_.RefreshPolicy;
            jObject["maxStack"] = (int)numeric_.MaxStack;
            jObject["duration"] = numeric_.Duration;
            return jObject;
        }
    }
}
