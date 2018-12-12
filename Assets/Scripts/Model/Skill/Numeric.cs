using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using Assets.Scripts.Model.UnitAttribute;

namespace Assets.Scripts.Model.Skill
{
    public class Numeric
    {
        enum ESubject
        {
            Source,
            Target,
            Trigger,
            Parent
        }

        enum EExtract
        {
            Current,
            Ratio,
            Percentage,
            Max,
            Missing,
            MissingRatio,
            MissingPercentage
        }

        float Base;
        bool HasReference;
        ESubject Subject;
        EExtract Extract;
        EUnitAttribute UnitAttribute;

        Numeric(float base_)
        {
            Base = base_;
            HasReference = false;
        }

        Numeric(float base_, ESubject subject_, EExtract extract_, EUnitAttribute unitAttribute_)
        {
            Base = base_;
            HasReference = true;
            Subject = subject_;
            Extract = extract_;
            UnitAttribute = unitAttribute_;
        }

        Numeric(JSONNode jNode_)
        {
            if (jNode_.IsNumber)
            {
                Base = jNode_["base"];
                HasReference = false;
            }
            else
            {
                Base = jNode_["base"];
                HasReference = true;
                Subject = (ESubject) jNode_["subject"].AsInt;
                Extract = (EExtract) jNode_["extract"].AsInt;
                UnitAttribute = (EUnitAttribute) jNode_["unitAttribute"].AsInt;
            }
        }

        public static implicit operator Numeric(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator JSONNode(Numeric numeric_)
        {
            if (!numeric_.HasReference)
            {
                return numeric_.Base;
            }
            else
            { 
                JSONObject jObject = new JSONObject();
                jObject["base"] = numeric_.Base;
                jObject["subject"] = (int) numeric_.Subject;
                jObject["extract"] = (int)numeric_.Extract;
                jObject["unitAttribute"] = (int) numeric_.UnitAttribute;
                return jObject;
            }
        }
    }
}
