using Assets.Scripts.Enemy.Pathfinding;
using Assets.Scripts.Util;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class ChillingState : NonDetectedState
    {
        public float patrollingSpeed = 200f;
        public float minDistance = 0.1f;
        public float searchingRadius = 5f;
        public float waitTimeWhenReach = 1f;

        private Vector2 initialPosition;
        private Rigidbody2D rb;
        private readonly float[] searchingLimits = new float[2];
        private int searchingIndex = 0;
        private readonly string _soundTag = "Sound";

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
            FakeStart();
            PlayChillingAnimation();
            UpdateSearchRadius(_lastRecivedContext);
            StartSearch();
            //Debug.Log("Entered chilling");
        }

        protected override void DoImplementation()
        {
            UpdateSearchRadius(_lastRecivedContext);
        }

        protected override void FixedDoImplementation()
        {
            var speed = patrollingSpeed;
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }

        public override void TriggerEnter(Collider2D collision)
        {
            HandleSoundCollition(collision);
        }

        private void UpdateSearchRadius(StateContext context)
        {
            //Debug.Log("Searching radius:" + searchingRadius);
            var x = initialPosition.x;
            searchingLimits[0] = x - searchingRadius;
            searchingLimits[1] = x + searchingRadius;
            
        }

        private void StartSearch()
        {
            var target = new Vector2(searchingLimits[searchingIndex], rb.position.y);
            pathfinder.SetDestination(target, SwitchTargets);
            //Debug.Log("Destination set");
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
            //StartSearch();
            WaitForNecessaryTime();
        }

        private void WaitForNecessaryTime()
        {
            _lastRecivedContext.WaitTime = (waitTimeWhenReach, EnemyStates.Chilling);
            parent.ChangeStates(EnemyStates.Waiting);    
        }

        private void HandleSoundCollition(Collider2D collision)
        {
            if (!Util.CollidedWithSound(parent.gameObject, collision)) return;
            var soundOrigin = collision.gameObject.transform.position;
            _lastRecivedContext.SoundPosition = soundOrigin;
            _lastRecivedContext.LevelManagerController.SoundWasHeard();
            parent.ChangeStates(EnemyStates.GoingAtSound,_lastRecivedContext);
        }

        private void PlayChillingAnimation()
        {
            animationController.Walking();
        }

    }

    
}
