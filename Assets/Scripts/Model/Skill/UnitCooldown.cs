using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Skill
{
    public class UnitCooldown
    {
        public NamedHash Type { get; private set; }

        public UnitCooldown(NamedHash type_)
        {
            Type = type_;
        }
    }
}
