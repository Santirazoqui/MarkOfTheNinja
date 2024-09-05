using Assets.Scripts.Enemy.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class ChillingState : State
    {
        Pathfinder pathfinder;
        public override void Do(StateContext context)
        {
            var cam = context.Parent.cam;
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pathfinder.SetDestination(mousePosition);
            }
        }

        public override void Enter(StateContext context)
        {
            pathfinder = gameObject.AddComponent<Pathfinder>();
        }

        public override void Exit(StateContext context)
        {
            //throw new NotImplementedException();
        }

        public override void FixedDo(StateContext context)
        {
            var speed = context.Parent.patrollingSpeed;
            var minDistance = context.Parent.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }
    }
}
