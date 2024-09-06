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
        private float[] searchingLimits = new float[2];
        private int searchingIndex = 0;
        public override void Do(StateContext context)
        {

            CheckForMouseSounds(context);
        }

        public override void Enter(StateContext context)
        {
            pathfinder = gameObject.AddComponent<Pathfinder>();
            rb = GetComponent<Rigidbody2D>();
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

        private void CheckForMouseSounds(StateContext context)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pathfinder.SetDestination(mousePosition, StartSearch);
                Debug.Log($"Mouse position: {mousePosition}");
            }
           
        }
        
        private void UpdateSearchRadius(StateContext context)
        {
            var searchingRadius = context.Parent.chillingSettings.searchingRadius;
            var x = transform.position.x;
            searchingLimits[0] = x - searchingRadius;
            searchingLimits[1] = x + searchingRadius;
        }

        private void StartSearch()
        {
            var target = new Vector2(searchingLimits[searchingIndex], rb.position.y);
            pathfinder.SetDestination(target, SwitchTargets);
            Debug.Log($"Starting search to position {target}");
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
