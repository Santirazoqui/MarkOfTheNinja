using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public abstract class DetectedStateV2:State
    {
        protected bool cantMove = false;

        public string playerTag = "Player";
        public string animationEventForKillingPlayer = "playerKilled";
        public string animationEventForThrowingFireball = "thowingFireballEnded";

        public override void CollitionEnter(Collision2D collision)
        {
            HandlePlayerCollition(collision.gameObject);
        }

        private void HandlePlayerCollition(GameObject player)
        {
            bool collidedWithPlayer = player.CompareTag(playerTag);
            if (!collidedWithPlayer) return;
            KillPlayer(player);
        }

        private void KillPlayer(GameObject player)
        {
            animationController.Killing();
            cantMove = true;
            player.SetActive(false);
        }

        public override void AnimationEventFired(string eventDescription)
        {
            if (eventDescription == animationEventForKillingPlayer)
            {
                PostKilling();
            }
            else if (eventDescription == animationEventForThrowingFireball)
            {
                cantMove = false;
                PlayDetectedAnimation();
            }
            
        }

        private void PostKilling()
        {
            levelManagerController.PublishEnemyStateChange(EnemyStates.Chilling);
        }


        protected override void EnterImplementation()
        {
            PlayDetectedAnimation();
        }

        private void PlayDetectedAnimation()
        {
            animationController.Walking();
        }


        protected bool CanSeePlayer()
        {
            var collider = player.GetComponent<Collider2D>();
            return !ObjectDetector.AnyObjectsBetween(parent.gameObject, collider);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            HandlePlayerCollition(collision.gameObject);
        }

    }
}
