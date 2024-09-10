using Assets.Scripts.Enemy.Pathfinding;
using Assets.Scripts.Sound;
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
        private Vector2 initialPosition;
        private Pathfinder pathfinder;
        private Rigidbody2D rb;
        private readonly float[] searchingLimits = new float[2];
        private int searchingIndex = 0;
        private readonly string _soundTag = "Sound";
        private EnemyController parent;

        private bool initiated = false;

        // Si en vez de poner el codigo en esta funcion, se pone en el start, tira null pointer exceptions 
        private void FakeStart()
        {
            if (initiated) return;
            rb = GetComponent<Rigidbody2D>();
            initialPosition = rb.position;
            pathfinder = _lastRecivedContext.Pathfinder;
            parent = _lastRecivedContext.Parent;
            initiated = true;
        }

        protected override void EnterImplementation()
        {
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
            var speed = _lastRecivedContext.chillingSettings.patrollingSpeed;
            var minDistance = _lastRecivedContext.chillingSettings.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }

        public override void TriggerEnter(Collider2D collision)
        {
            HandleSoundCollition(collision);
        }

        private void UpdateSearchRadius(StateContext context)
        {
            var searchingRadius = context.Parent.chillingSettings.searchingRadius;
            Debug.Log("Searching radius:" + searchingRadius);
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
            Debug.Log("Switch targets called");
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

        private void HandleSoundCollition(Collider2D collision)
        {
            bool collidedWithASound = collision.gameObject.CompareTag(_soundTag);
            if (!collidedWithASound) return;
            var soundOrigin = collision.gameObject.transform.position;
            _lastRecivedContext.SoundPosition = soundOrigin;
            parent.ChangeStates(EnemyController.EnemyStates.GoingAtSound,_lastRecivedContext);
        }


    }

    
}
