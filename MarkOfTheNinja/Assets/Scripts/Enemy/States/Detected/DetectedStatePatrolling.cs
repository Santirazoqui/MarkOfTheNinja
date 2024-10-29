using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.States.Detected
{
    public class DetectedStatePatrolling:DetectedStateV2
    {

        public float patrollingSpeed = 300f;
        public float persectutionSpeed = 400f;
        public float searchingRadius = 500f;

        private Vector2 initialPosition;
        private Rigidbody2D rb;
        private readonly float[] searchingLimits = new float[2];
        private int searchingIndex = 0;

        private bool initiated = false;

        // Si en vez de poner el codigo en esta funcion, se pone en el start, tira null pointer exceptions 
        private void FakeStart()
        {
            if (initiated) return;

            pathfinder = _lastRecivedContext.Pathfinder;
            parent = _lastRecivedContext.Parent;

            rb = parent.GetComponent<Rigidbody2D>();
            initialPosition = rb.position;
            initiated = true;
            animationController = _lastRecivedContext.AnimationController;
        }

        protected override void EnterImplementation()
        {
            base.EnterImplementation();
            FakeStart();
            UpdateSearchRadius(_lastRecivedContext);
            StartSearch();
        }

        protected override void DoImplementation()
        {
            UpdateSearchRadius(_lastRecivedContext);
        }

        protected override void FixedDoImplementation()
        {
            base.FixedDoImplementation();
            if(CanSeePlayer())
            {
                Hunt();
            }
            else
            {
                StartSearch();
                Patroll();
            }
        }

        private void Hunt()
        {
            pathfinder.SetDestination(player.transform.position, () => { });
            var speed = persectutionSpeed;
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }


        private void Patroll()
        {
            var speed = patrollingSpeed;
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }


        private void UpdateSearchRadius(StateContext context)
        {
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
            //Debug.Log($"Switch targets called at: {parent.transform.position.x},{parent.transform.position.y}");
            if (searchingLimits.Length - 1 == searchingIndex)
            {
                searchingIndex = 0;
            }
            else
            {
                searchingIndex++;
            }
            StartSearch();
        }





    }
}
