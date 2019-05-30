using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Skill
{
    public class Input
    {
        public enum ETrigger
        {
            Channel,
            Release,
        }

        public ETrigger Trigger { get; private set; }
        public SkillMetricReference HoldMaxDuration { get; private set; }
        public bool CastOnHoldExpire { get; private set; }
        public InputVisual.Base Visual { get; private set; }
        public Condition Condition { get; private set; }
        public List<Effect.Base> Effects { get; private set; }

        public Input(
            ETrigger trigger_,
            SkillMetricReference holdMaxDuration_,
            bool castOnHoldExpire_,
            )
        {

        }
    }
}
