using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Context
{
    public class PlayerNew : MonoBehaviour
    {
        public float movementSpeed = 50.0f;
        public float cursorSensitivity = 5.0f;
        public Controller.ActionController actionController;
        public float rawraw = 0;

        float horizontal = 0;
        float vertical = 0;

        List<Skill.IAction> currentActions = new List<Skill.IAction>();
        Skill.IAction dashForward;
        Skill.IAction dashBackward;
        Skill.IAction dashLeft;
        Skill.IAction dashRight;

        // Use this for initialization
        void Start()
        {
            dashForward = new Skill.DashForward(this);
            dashBackward = new Skill.DashBackward(this);
            dashLeft = new Skill.DashLeft(this);
            dashRight = new Skill.DashRight(this);

            actionController.SetForwardDashAction(() => { CastAction(dashForward); });
            actionController.SetBackwardDashAction(() => { CastAction(dashBackward); });
            actionController.SetLeftDashAction(() => { CastAction(dashLeft); });
            actionController.SetRightDashAction(() => { CastAction(dashRight); });
        }

        // Update is called once per frame
        void Update()
        {
            bool canMove = true;
            for (int i = currentActions.Count - 1; i >= 0; i--)
            {
                if (!currentActions[i].Update())
                {
                    currentActions.RemoveAt(i);
                }
                else if (currentActions[i].MovementLocked())
                {
                    canMove = false;
                }
            }

            if (canMove) {
                vertical = Input.GetAxisRaw("Vertical");
                horizontal = Input.GetAxisRaw("Horizontal");
                if (vertical != 0 || horizontal != 0)
                {
                    Vector3 axis = new Vector3(vertical, 0, -horizontal);
                    float axisSpeed = Mathf.Min(1.0f, axis.magnitude);
                    transform.position += transform.rotation * axis * movementSpeed * Time.deltaTime / axisSpeed;
                }
            }

            transform.Rotate(0, Input.GetAxis("Mouse X") * cursorSensitivity, 0);
        }

        void CastAction(Skill.IAction action_)
        {
            foreach (var otherAction in currentActions)
            {
                if (otherAction.ActionLocked() || otherAction == action_)
                {
                    return;
                }
            }

            action_.Cast();
            currentActions.Add(action_);
        }
    }
}