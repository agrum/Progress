using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill
{
    public class Modifier
    {
        public enum EExtract
        {
            Stack,
            Duration,
        }

        public Stacker Stacker { get; private set; }
        public List<ModifierBehaviour> Behaviours { get; private set; } = new List<ModifierBehaviour>();

        public Modifier(params ModifierBehaviour[] behaviours_)
            : this(new Stacker(), behaviours_)
        {

        }

        public Modifier(Stacker stacker_, params ModifierBehaviour[] behaviours_)
        {
            Stacker = stacker_;
            Behaviours = new List<ModifierBehaviour>(behaviours_);
        }

        public Modifier(JSONNode jNode_)
        {
            Stacker = jNode_["Stacker"];
            foreach (var behaviour in jNode_["Behaviours"].AsArray)
                Behaviours.Add(behaviour.Value);
        }

        public static implicit operator JSONNode(Modifier modifier_)
        {
            JSONObject jObject = new JSONObject();
            jObject["Stacker"] = modifier_.Stacker;
            JSONArray behaviours = new JSONArray();
            foreach (var behaviour in modifier_.Behaviours)
                behaviours.Add(behaviour);
            jObject["Behaviours"] = behaviours;
            return jObject;
        }
    }
}
