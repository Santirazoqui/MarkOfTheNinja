using Assets.Scripts.Enemy.Pathfinding;
using Pathfinding;
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
                Debug.Log("Mouse input was detected");
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pathfinder.SetDestination(mousePosition);
                Debug.Log("Pathfinding set destination was called");
            }
        }

        public override void Enter(StateContext context)
        {
            pathfinder = gameObject.AddComponent<Pathfinder>();
            pathfinder.seeker = context.Parent.GetComponent<Seeker>();
            Debug.Log("Entered chilling");
        }

        public override void Exit(StateContext context)
        {
            //throw new NotImplementedException();
        }

        public override void FixedDo(StateContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
