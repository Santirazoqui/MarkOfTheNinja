using Assets.Scripts.Enemy.Pathfinding;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class DetectedState : State
    {
        public float persectutionSpeed = 300f;
        public float minDistance = 0.1f;
        private bool killingPlayer = false;
        
        public string playerTag = "Player";
        public string animationEventForKillingPlayer = "playerKilled";

        public override void CollitionEnter(Collision2D collision)
        {
            HandlePlayerCollition(collision.gameObject);
        }

        private void HandlePlayerCollition(GameObject player)
        {
            bool collidedWithPlayer = player.CompareTag(playerTag);
            if (!collidedWithPlayer) return;
            KillPlayer(player.gameObject);
        }

        private void KillPlayer(GameObject player)
        {
            animationController.Killing();
            killingPlayer = true;
            player.SetActive(false);
        }

        public override void AnimationEventFired(string eventDescription)
        {
            if (eventDescription != animationEventForKillingPlayer) return;
            PostKilling();
        }

        private void PostKilling()
        {
            levelManagerController.PublishEnemyStateChange(EnemyStates.Chilling);
        }


        protected override void EnterImplementation()
        {
            pathfinder.IgnoreEnenmyWallsCollitions = true;
            PlayDetectedAnimation();
        }

        private void PlayDetectedAnimation()
        {
            animationController.Walking();
        }

        protected override void FixedDoImplementation()
        {
            if (killingPlayer)
            {
                pathfinder.AdjustPosition(0, 0);
                return;
            }
            pathfinder.SetDestination(player.transform.position, DoNothing);
            var speed = persectutionSpeed;
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed,minDistance);
        }




        private void DoNothing() { }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HandlePlayerCollition(collision.gameObject);
        }


    }
}
