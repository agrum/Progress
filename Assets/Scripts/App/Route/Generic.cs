using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Route
{
    public class Generic
    {
        public delegate object RouteDelegate(object object_, System.Collections.IEnumerator enumerator_);

        public Modifier Modifier { get; private set; } = new Modifier();
        public Trigger Trigger { get; private set; } = new Trigger();
        public Unit Unit { get; private set; } = new Unit();

        public Generic()
        {

        }
    }
}
