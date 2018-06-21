using UnityEngine;

namespace West.View
{
    public abstract class WestBehaviour : MonoBehaviour
    {
        public delegate void OnDelayedDelegate();

        private bool started = false;
        private OnDelayedDelegate onDelayed = null;

        void Start()
        {
            WestStart();

            onDelayed?.Invoke();
            onDelayed = null;

            started = true;
        }

        public void Delay(OnDelayedDelegate onDelayed_)
        {
            if (started)
                onDelayed_();
            else 
                onDelayed = onDelayed_;
        }

        protected virtual void WestStart()
        {

        }
    }
}
