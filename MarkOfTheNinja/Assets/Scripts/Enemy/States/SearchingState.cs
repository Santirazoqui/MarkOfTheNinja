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

        protected override void _Do()
        {
            //throw new NotImplementedException();
        }

        protected override void _Enter()
        {
            pathfinder = _lastRecivedContext.Pathfinder;
            parent = _lastRecivedContext.Parent;

            pathfinder.SetDestination(_lastRecivedContext.SoundPosition, ChangeBackToChilling);
        }

        protected override void _Exit()
        {
            
        }

        protected override void _FixedDo()
        {
            var speed = _lastRecivedContext.Parent.searchingSettings.searchingSpeed;
            var minDistance = _lastRecivedContext.Parent.searchingSettings.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }

        private void ChangeBackToChilling()
        {
            parent.ChangeStates(EnemyController.EnemyStates.Chilling);
        }
    }
}
