using SimpleJSON;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Context.Skill
{
    public class Dash : IAction
    {
        Data.Skill.Skill data = Asset.SkillExport.Dash.GetData();
        Vector3 relativeDirection;
        Vector3 direction;
        readonly PlayerNew player;
        float playerSpeed;

        float remainingDistance;
        float travelDistance;
        float maxSpeedMultiplier;
        float minSpeedMultiplier;

        public Dash(Vector3 direction_, PlayerNew player_)
        {
            relativeDirection = direction_.normalized;
            player = player_;
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
            Context context = new Context();
            travelDistance = remainingDistance = (float) data.GetMetric(Asset.SkillExport.Dash.Range).Numeric.Get(context);
            maxSpeedMultiplier = (float) data.GetMetric(Asset.SkillExport.Dash.MaxSpeedMultiplier).Numeric.Get(context);
            minSpeedMultiplier = (float) data.GetMetric(Asset.SkillExport.Dash.MinSpeedMultiplier).Numeric.Get(context);
            playerSpeed = player.movementSpeed;
            direction = player.transform.rotation * relativeDirection.normalized;
        }

        public override bool Update()
        {
            float dashSpeed = Mathf.Lerp(maxSpeedMultiplier, minSpeedMultiplier, remainingDistance / travelDistance);
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