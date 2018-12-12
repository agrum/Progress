using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Unit
    {
        public Vector3 Position { get; private set; }

        List<Modifier> ModifierList = new List<Modifier>();

        UnitTicker ticker = new UnitTicker();

        static public void BuildReferencePath(System.Collections.IEnumerator stringEnumerator, ref List<NumericValueSkill.ReferencePath> referencePat)
        {
            var node = stringEnumerator.Current as string;

        }

        public float Reference(List<object> referencePath_, int index_)
        {
            return 0;
        }

        public void ApplyModifier(Modifier modifier_)
        {
            ModifierList.Add(modifier_);

            ticker.ListensTo(modifier_);
        }

        public void RemoveModifier(Modifier sourceModifier_, string idName_)
        {
            int index = 0;
            foreach (var modifier in ModifierList)
            {
                if (modifier.SourceModifier == sourceModifier_ && modifier.IdName == idName_)
                {
                    modifier.Kill();
                    ModifierList.RemoveAt(index);
                    break;
                }
                ++index;
            }
        }

        void Tick(float time_)
        {
            //tick throught all attached modifiers and kill the expired ones
            foreach (var modifier in ModifierList)
                if (modifier.ExpirationTime <= time_)
                    modifier.Kill();

            //tick through ticker modifiers
            ticker.Tick(time_);
        }
    }
}
