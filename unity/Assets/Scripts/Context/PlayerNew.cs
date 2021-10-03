using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Context
{
    public class PlayerNew : MonoBehaviour
    {
        public Scene.Session session;
        public float movementSpeed = 50.0f;
        public float cursorSensitivity = 5.0f;
        public Controller.ActionController actionController;

        float horizontal = 0;
        float vertical = 0;

        readonly List<Skill.IAction> currentActions = new List<Skill.IAction>();
        Skill.IAction dashForward;
        Skill.IAction dashBackward;
        Skill.IAction dashLeft;
        Skill.IAction dashRight;

        Rigidbody2D rb2d;

        // Use this for initialization
        IEnumerator Start()
        {
            yield return StartCoroutine(App.Content.SkillList.Load());

            yield return StartCoroutine(session.Load());

            dashForward = new Skill.DashForward(this);
            dashBackward = new Skill.DashBackward(this);
            dashLeft = new Skill.DashLeft(this);
            dashRight = new Skill.DashRight(this);

            actionController.SetForwardDashAction(() => { CastAction(dashForward); });
            actionController.SetBackwardDashAction(() => { CastAction(dashBackward); });
            actionController.SetLeftDashAction(() => { CastAction(dashLeft); });
            actionController.SetRightDashAction(() => { CastAction(dashRight); });

            rb2d = gameObject.GetComponent<Rigidbody2D>();
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
                    Vector3 axis = new Vector3(horizontal, vertical, 0);
                    if (axis.sqrMagnitude > 0)
                    {
                        var delta = transform.rotation * axis.normalized * movementSpeed * Mathf.Max(Mathf.Abs(vertical), Mathf.Abs(horizontal));
                        rb2d.MovePosition(transform.position + delta);
                    }
                }
            }

            transform.Rotate(0, 0, Input.GetAxis("Mouse X") * -cursorSensitivity);
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