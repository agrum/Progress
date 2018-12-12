using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Route
{
    public class Trigger
    {
        enum EAttribute
        {
            Unit,
        }

        private Dictionary<EAttribute, Generic.RouteDelegate> ReferencePathDictionary = new Dictionary<EAttribute, Generic.RouteDelegate>();

        public Trigger()
        {
            ReferencePathDictionary.Add(EAttribute.Unit, ReferenceUnit);
        }
        //GENERIC
        public object Reference(object object_, System.Collections.IEnumerator enumerator_)
        {
            EAttribute? attribute = enumerator_.Current as EAttribute?;
            if (!attribute.HasValue)
                throw new Exception();
            return ReferencePathDictionary[attribute.Value](object_, enumerator_);
        }

        //NESTING
        public object ReferenceUnit(object object_, System.Collections.IEnumerator enumerator_)
        {
            Model.Trigger trigger = object_ as Model.Trigger;
            return App.Route.Unit.Reference(trigger.Unit, GetNextAttribute(enumerator_, true));
        }
        
        private System.Collections.IEnumerator GetNextAttribute(System.Collections.IEnumerator enumerator_, bool needNext_)
        {
            if (enumerator_.MoveNext() == needNext_)
                throw new Exception();
            return enumerator_;
        }
    }
}