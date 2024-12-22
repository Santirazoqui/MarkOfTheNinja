using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class StunedState:NonDetectedState
    {
        [SerializeField] AudioClip playerHeadStompClip;
        public float distanceItSeachesBehindAfterStunEnds;
        private AudioSource audioSource;
        private readonly string stunnedAnimationEnd = "stumpEnded";

        private void Awake()
        {
            audioSource = GetComponentInParent<AudioSource>();
        }
        protected override void EnterImplementation()
        {
            animationController.Stunned();
            pathfinder.AdjustPosition(0, 0);
            pathfinder.Waiting = true;
            audioSource.PlayOneShot(playerHeadStompClip);
            visionCone.gameObject.SetActive(false);
            foreach (var item in instaDetectionHitboxes)
            {
                item.gameObject.SetActive(false);
            }
            //Debug.Log($"Waiting for {waitTime}");
        }

        public override void AnimationEventFired(string eventDescription)
        {
            base.AnimationEventFired(eventDescription);
            if(eventDescription == stunnedAnimationEnd)
            {
                detectionRateManager.SoundWasHeard();
                TurnAround();
            }
        }

        private void TurnAround()
        {
            var enemyDirection = Math.Sign(parent.gameObject.transform.localScale.x);
            var xPosition = parent.gameObject.transform.position.x - enemyDirection * distanceItSeachesBehindAfterStunEnds;
            _lastRecivedContext.SoundPosition = new Vector2(xPosition,0); // y postion currently doesnt matter
            _lastRecivedContext.Speed = searchingAtSoundSpeed;
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.GoingAtSound, _lastRecivedContext);
        }

        protected override void ExitImplementation()
        {
            pathfinder.Waiting = false;
            visionCone.gameObject.SetActive(true);
            foreach (var item in instaDetectionHitboxes)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
