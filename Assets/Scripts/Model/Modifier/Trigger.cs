using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class Trigger
    {
        public enum EType
        {
            Tick,
            UnitEnter,
            UntiLeave,
        }

        public EType Type { get; private set; }
        public Unit Unit { get; private set; }

        public Trigger(EType type_, Unit unit_)
        {
            Type = type_;
            Unit = unit_;
        }

        static public void BuildReferencePath(string[] stringPath, int index_, ref List<object> referencePath)
        {
            string node = stringPath[index_];
            
        }

        public float Reference(List<object> referencePath_, int index_)
        {
            return 0;
        }
}
