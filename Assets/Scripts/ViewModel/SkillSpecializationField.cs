using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializationField
    {
        public event OnVoidDelegate SpecLevelChanged = delegate { };

        public enum SpecializeSign
        {
            Positive,
            Negative,
            None
        }

        public bool Editable()
        {
            return true;
        }

        public SpecializeSign Sign()
        {
            return SpecializeSign.None;
        }

        public float SpecLevel()
        {
            return 0.0f;
        }

        public float NextSpecLevel()
        {
            return 0.0f;
        }

        public void Buy(SpecializeSign sign)
        {

        }
    }
}
