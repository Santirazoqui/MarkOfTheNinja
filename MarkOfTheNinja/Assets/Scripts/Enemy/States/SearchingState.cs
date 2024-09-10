using Assets.Scripts.Enemy.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class SearchingState : State
    {
        private Pathfinder pathfinder;
        private EnemyController parent;
        private readonly string _soundTag = "Sound";

        public override void TriggerEnter(Collider2D collision)
        {
            HandleSoundCollition(collision);
        }

        private void HandleSoundCollition(Collider2D collision)
        {
            bool collidedWithASound = collision.gameObject.CompareTag(_soundTag);
            if (!collidedWithASound) return;
            var soundOrigin = collision.gameObject.transform.position;
            pathfinder.SetDestination(soundOrigin, ChangeBackToChilling);
        }


        protected override void EnterImplementation()
        {
            pathfinder = _lastRecivedContext.Pathfinder;
            parent = _lastRecivedContext.Parent;

            pathfinder.SetDestination(_lastRecivedContext.SoundPosition, ChangeBackToChilling);
        }

        protected override void FixedDoImplementation()
        {
            var speed = _lastRecivedContext.searchingSettings.searchingSpeed;
            var minDistance = _lastRecivedContext.searchingSettings.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }

        private void ChangeBackToChilling()
        {
            parent.ChangeStates(EnemyController.EnemyStates.Chilling);
        }
    }
}
