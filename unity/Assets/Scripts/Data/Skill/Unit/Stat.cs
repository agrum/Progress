using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.Skill.Unit
{
    public class Stat
    {
        public enum ECategory
        {
            None = 0,
            Physical = 1,
            Magical = 2,
            Heal = 4,
            True = 8,
        }

        public enum EStandard
        {
            Power,
            Defense,
            MovementSpeed,
            Haste,
            CriticalProbability,
            CriticalMultiplier,
        } 
    }
}
