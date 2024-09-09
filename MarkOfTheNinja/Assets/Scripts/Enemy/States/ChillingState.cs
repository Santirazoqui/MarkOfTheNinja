using Assets.Scripts.Enemy.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

namespace Assets.Scripts.Enemy.States
{
    public class ChillingState : State
    {
        private Pathfinder pathfinder;
        private Rigidbody2D rb;
        private readonly float[] searchingLimits = new float[2];
        private int searchingIndex = 0;
        private Vector2 initialPosition;
        public override void Do(StateContext context)
        {
            UpdateSearchRadius(context);
            
        }

        public override void Enter(StateContext context)
        {
            pathfinder = gameObject.AddComponent<Pathfinder>();
            rb = GetComponent<Rigidbody2D>();
            initialPosition = rb.position;
            UpdateSearchRadius(context);
            StartSearch();
        }


        public override void Exit(StateContext context)
        {
            //throw new NotImplementedException();
        }

        public override void FixedDo(StateContext context)
        {
            var speed = context.Parent.chillingSettings.patrollingSpeed;
            var minDistance = context.Parent.chillingSettings.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }


        
        
        private void UpdateSearchRadius(StateContext context)
        {
            var searchingRadius = context.Parent.chillingSettings.searchingRadius;
            var x = initialPosition.x;
            searchingLimits[0] = x - searchingRadius;
            searchingLimits[1] = x + searchingRadius;
            
        }

        private void StartSearch()
        {
            var target = new Vector2(searchingLimits[searchingIndex], rb.position.y);
            pathfinder.SetDestination(target, SwitchTargets);
        }

        private void SwitchTargets()
        {
            if (searchingLimits.Length - 1 == searchingIndex)
            {
                searchingIndex = 0;
            }
            else
            {
                searchingIndex++;
            }
            Debug.Log($"Switch to position {searchingLimits[searchingIndex]}");
            StartSearch();
        }

    }

    
}
