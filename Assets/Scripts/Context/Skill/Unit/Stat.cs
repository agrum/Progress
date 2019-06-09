using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Context.Skill.Unit
{
    public class Stat
    {
        public Data.NamedHash Type { get; private set; }
        private float Additive = 0;
        private float AdditiveMultiplier = 0;
        private float Multiplier = 1;
        private float Zeroes = 0;
        public float Value { get; private set; } = 0;

        public Stat(Data.NamedHash type_)
        {
            Type = type_;
        }

        public void Input(float value_, Data.Skill.Unit.Stat.ECategory category_)
        {

        }
    }
}
