using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill
{
    public abstract class TriggerType
    {
        public enum EType
        {
            InputSkillDown,
            InputSkillUp,
            InputMove,
            Begin,
            Tick,
            End,
            StackAdded,
            StackRemoved,
            StackChanged,
            UnitCreated,
            UnitDestroyed,
            ModifierApplied,
            ModifierRemoved,
            EnteredRadius,
            LeftRadius,
            HitDealt,
            HitReceived,
            Custom
        }

        public EType Type { get; protected set; }

        public abstract JSONObject ToJson();
        
        public static implicit operator TriggerType(JSONNode jNode_)
        {
            switch((EType) jNode_["type"].AsInt)
            {
                case EType.Begin: return new TriggerBegin();
                case EType.Custom: return new TriggerCustom();
                case EType.End: return new TriggerEnd();
                case EType.EnteredRadius: return new TriggerEnteredRadius();
                case EType.HitDealt: return new TriggerHitDealt();
                case EType.HitReceived: return new TriggerHitReceived();
                case EType.InputMove: return new TriggerInputMove();
                case EType.LeftRadius: return new TriggerLeftRadius();
                case EType.ModifierApplied: return new TriggerModifierApplied();
                case EType.ModifierRemoved: return new TriggerModifierRemoved();
                case EType.StackAdded: return new TriggerStackAdded();
                case EType.StackChanged: return new TriggerStackChanged();
                case EType.StackRemoved: return new TriggerStackRemoved();
                case EType.Tick: return new TriggerTick();
                case EType.UnitCreated: return new TriggerUnitCreated();
                case EType.UnitDestroyed: return new TriggerUnitDestroyed();
                default: return null;
            }
        }

        public static implicit operator JSONNode(TriggerType triggerType_)
        {
            JSONObject jObject = triggerType_.ToJson();
            jObject["type"] = (int)triggerType_.Type;
            return jObject;
        }

    public class TriggerInputSkillDown : TriggerType
    {
        public TriggerInputSkillDown()
        {
            Type = EType.InputSkillDown;
        }

        public override JSONObject ToJson()
        {
            return null;
        }
    }

    public class TriggerInputSkillUp : TriggerType
    {
        public TriggerInputSkillUp()
        {
            Type = EType.InputSkillUp;
        }
    }

    public class TriggerInputMove : TriggerType
    {
        public TriggerInputMove()
        {
            Type = EType.InputMove;
        }
    }

    public class TriggerBegin : TriggerType
    {
        public TriggerBegin()
        {
            Type = EType.Begin;
        }
    }

    public class TriggerTick : TriggerType
    {
        public TriggerTick()
        {
            Type = EType.Tick;
        }
    }

    public class TriggerEnd : TriggerType
    {
        public TriggerEnd()
        {
            Type = EType.End;
        }
    }

    public class TriggerStackAdded : TriggerType
    {
        public TriggerStackAdded()
        {
            Type = EType.StackAdded;
        }
    }

    public class TriggerStackRemoved : TriggerType
    {
        public TriggerStackRemoved()
        {
            Type = EType.StackRemoved;
        }
    }

    public class TriggerStackChanged : TriggerType
    {
        public TriggerStackChanged()
        {
            Type = EType.StackChanged;
        }
    }

    public class TriggerUnitCreated : TriggerType
    {
        public TriggerUnitCreated()
        {
            Type = EType.UnitCreated;
        }
    }

    public class TriggerUnitDestroyed : TriggerType
    {
        public TriggerUnitDestroyed()
        {
            Type = EType.UnitDestroyed;
        }
    }

    public class TriggerModifierApplied : TriggerType
    {
        public TriggerModifierApplied()
        {
            Type = EType.ModifierApplied;
        }
    }

    public class TriggerModifierRemoved : TriggerType
    {
        public TriggerModifierRemoved()
        {
            Type = EType.ModifierRemoved;
        }
    }

    public class TriggerEnteredRadius : TriggerType
    {
        public TriggerEnteredRadius()
        {
            Type = EType.EnteredRadius;
        }
    }

    public class TriggerLeftRadius : TriggerType
    {
        public TriggerLeftRadius()
        {
            Type = EType.LeftRadius;
        }
    }

    public class TriggerHitDealt : TriggerType
    {
        public TriggerHitDealt()
        {
            Type = EType.HitDealt;
        }
    }

    public class TriggerHitReceived : TriggerType
    {
        public TriggerHitReceived()
        {
            Type = EType.HitReceived;
        }
    }

    public class TriggerCustom : TriggerType
    {
        public TriggerCustom()
        {
            Type = EType.Custom;
        }
    }
}
