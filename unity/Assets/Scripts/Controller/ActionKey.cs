using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public delegate void OnKeyPressed();

    public class ActionKey
    {
        public event OnKeyPressed Single = delegate { };
        public event OnKeyPressed Double = delegate { };

        private static float doublePressTimeTreshold = 0.5f;
        private KeyCode monitoredKey;
        private float timeSinceLastPressed = Mathf.Infinity;

        public void SetKey(KeyCode key)
        {
            monitoredKey = key;
        }

        public void Update()
        {
            timeSinceLastPressed += Time.deltaTime;
            if (Input.GetKeyDown(monitoredKey))
            {
                if (timeSinceLastPressed < doublePressTimeTreshold)
                {
                    Double();
                }
                Single();
                timeSinceLastPressed = 0.0f;
            }
        }
    }
}