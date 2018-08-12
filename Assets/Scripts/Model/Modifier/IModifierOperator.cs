using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public delegate void OnTriggerCreated(Trigger trigger_);

    public interface IModifierOperator
    {
        event OnTriggerCreated TriggerCreated;

        void Update();
    }
}
