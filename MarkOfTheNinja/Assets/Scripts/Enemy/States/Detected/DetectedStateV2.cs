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
        private bool killingPlayer = false;
        private int frameCounter = 0;

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
            KillPlayer(player);
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
                frameCounter++;
                Debug.Log($"Killing player {frameCounter}");
                return;
            }
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
