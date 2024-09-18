using Assets.Scripts.Enemy.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class SearchingState : State
    {
        public float searchingSpeed = 300f;
        public float minDistance = 0.1f;


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
            var speed = this.searchingSpeed;
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }

        private void ChangeBackToChilling()
        {
            parent.ChangeStates(EnemyStates.Chilling);
        }
    }
}
