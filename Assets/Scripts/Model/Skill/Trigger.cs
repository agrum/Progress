using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Skill
{
    public class Trigger
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

        public EType Type { get; private set; }

        public Trigger(EType type_)
        {
            Type = type_;
        }
    }
}
