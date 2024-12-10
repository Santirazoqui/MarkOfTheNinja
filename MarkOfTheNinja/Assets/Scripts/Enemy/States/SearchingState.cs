using Assets.Scripts.Enemy.Pathfinding;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class SearchingState : NonDetectedState
    {
        //public float searchingSpeed = 300f;
        [SerializeField] AudioClip notFoundClip;
        [SerializeField] AnimationClip searchingAnimation;
        AudioSource audioSource;

        private readonly string _soundTag = "Sound";

        void Start()
        {
            audioSource = GetComponentInParent<AudioSource>();
        }

        protected override void EnterImplementation()
        {
            pathfinder = _lastRecivedContext.Pathfinder;
            parent = _lastRecivedContext.Parent;

            pathfinder.SetDestination(_lastRecivedContext.SoundPosition, StartSearchAtSound);
            PlaySearchingAnimation();
            audioSource.clip = notFoundClip;
            audioSource.PlayScheduled(AudioSettings.dspTime + searchingAnimation.length + 1);
            //Debug.Log(searchingAnimation.length);
        }

        private void PlaySearchingAnimation()
        {
            _lastRecivedContext.AnimationController.AlertWalking();
        }

        protected override void FixedDoImplementation()
        {
            var speed = _lastRecivedContext.Speed;
            //Debug.Log($"Speed: {speed}");
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }

        private void StartSearchAtSound()
        {
            parent.ChangeStates(EnemyStates.SearchingAtSound);
        }
    }
}
