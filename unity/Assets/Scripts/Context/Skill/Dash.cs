using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Context.Skill
{
    public class Dash : IAction
    {
        static float travelDistance = 5.0f;
        static float dashMinSpeedModifier = 2.0f;
        static float dashMaxSpeedModifier = 6.0f;

        Vector3 direction;
        PlayerNew player;
        float playerSpeed;
        float remainingDistance = travelDistance;

        public Dash(Vector3 direction_, PlayerNew player_)
        {
            direction = player_.transform.rotation * direction_.normalized;
            player = player_;
            playerSpeed = player.movementSpeed;
        }
        public override bool MovementLocked()
        {
            return true;
        }

        public override bool ActionLocked()
        {
            return false;
        }

        public override void Cast()
        {
            remainingDistance = travelDistance;
        }

        public override bool Update()
        {
            float dashSpeed = Mathf.Lerp(dashMaxSpeedModifier, dashMinSpeedModifier, remainingDistance / travelDistance);
            float travel = Mathf.Min(remainingDistance, playerSpeed * dashSpeed * Time.deltaTime);
            player.transform.position += direction * travel;
            remainingDistance -= travel;

            return remainingDistance > 0;
        }
    }

    public class DashForward : Dash
    {
        public DashForward(PlayerNew player_)
            : base(new Vector3(1, 0, 0), player_)
        {
        }
    }

    public class DashBackward : Dash
    {
        public DashBackward(PlayerNew player_)
            : base(new Vector3(-1, 0, 0), player_)
        {
        }
    }

    public class DashLeft : Dash
    {
        public DashLeft(PlayerNew player_)
            : base(new Vector3(0, 0, 1), player_)
        {
        }
    }

    public class DashRight : Dash
    {
        public DashRight(PlayerNew player_)
            : base(new Vector3(0, 0, -1), player_)
        {
        }
    }
}