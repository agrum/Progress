using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Context.Skill
{
    public abstract class IAction
    {
        public abstract bool MovementLocked();
        public abstract bool ActionLocked();
        public abstract void Cast();
        public abstract bool Update();
    }
}