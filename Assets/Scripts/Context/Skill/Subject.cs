using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.Skill
{
    static class Subject
    {
        public static Context.Skill.Container GetContainer(this ESubject subject_, Context.Skill.TriggerInfo triggerInfo_)
        {
            switch (subject_)
            {
                case ESubject.Target: return triggerInfo_.Source;
                case ESubject.Source: return (triggerInfo_.Source != null) ? triggerInfo_.Source : triggerInfo_.Target;
                case ESubject.Trigger: return (triggerInfo_.Trigger != null) ? triggerInfo_.Trigger : triggerInfo_.Target;
                case ESubject.Parent: return (triggerInfo_.Target.Parent != null) ? triggerInfo_.Target.Parent : triggerInfo_.Target;
            }

            return triggerInfo_.Target;
        }
    }
}
