using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model
{
    public class Modifier
    {
        enum EAlignment
        {
            Good,
            Neutral,
            Bad,
        }

        public bool ToDestroy { get; private set; } = false;

        Skill SourceSkill;
        public Modifier SourceModifier { get; private set; }
        Unit AttachedUnit;
        public string IdName { get; private set; }
        bool Removable;
        EAlignment Alignment;
        int MaxStack;
        float Duration;
        public float ExpirationTime { get; private set; }
        public float TickPeriod { get; private set; }
        public bool Ticks { get; private set; } = true;

        public Modifier(Skill sourceSkill_, Modifier sourceModifier_, Unit attachedUnit_, JSONObject json_)
        {
            SourceSkill = sourceSkill_;
            SourceModifier = sourceModifier_;
            AttachedUnit = attachedUnit_;

            if (TickPeriod > 0)
                Ticks = true;
            AttachedUnit.ApplyModifier(this);
        }

        public void Tick()
        {

        }

        public void Kill()
        {
            ToDestroy = true;
        }
    }
}
