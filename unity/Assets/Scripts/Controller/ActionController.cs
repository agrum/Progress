using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class ActionController : MonoBehaviour
    {
        private ActionKey moveForwardKey = new ActionKey();
        private ActionKey moveBackwardKey = new ActionKey();
        private ActionKey moveLeftKey = new ActionKey();
        private ActionKey moveRightKey = new ActionKey();

        // Use this for initialization
        void Start()
        {
            moveForwardKey.SetKey(KeyCode.W);
            moveBackwardKey.SetKey(KeyCode.S);
            moveLeftKey.SetKey(KeyCode.A);
            moveRightKey.SetKey(KeyCode.D);
        }

        // Update is called once per frame
        void Update()
        {
            moveForwardKey.Update();
            moveBackwardKey.Update();
            moveLeftKey.Update();
            moveRightKey.Update();
        }

        public void SetForwardDashAction(OnKeyPressed action)
        {
            moveForwardKey.Double += action;
        }

        public void SetBackwardDashAction(OnKeyPressed action)
        {
            moveBackwardKey.Double += action;
        }

        public void SetLeftDashAction(OnKeyPressed action)
        {
            moveLeftKey.Double += action;
        }

        public void SetRightDashAction(OnKeyPressed action)
        {
            moveRightKey.Double += action;
        }
    }
}