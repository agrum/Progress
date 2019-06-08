using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Context.Skill.Unit
{
    public class Cooldown
    {
        public Data.NamedHash Type { get; private set; }

        public Cooldown(Data.NamedHash type_)
        {
            Type = type_;
        }
    }
}
