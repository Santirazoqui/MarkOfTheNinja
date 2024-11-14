using Assets.Scripts.Enemy.Pathfinding;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class SearchingState : NonDetectedState
    {
        //public float searchingSpeed = 300f;

        private readonly string _soundTag = "Sound";

        protected override void EnterImplementation()
        {
            pathfinder = _lastRecivedContext.Pathfinder;
            parent = _lastRecivedContext.Parent;

            pathfinder.SetDestination(_lastRecivedContext.SoundPosition, StartSearchAtSound);
            PlaySearchingAnimation();
        }

        private void PlaySearchingAnimation()
        {
            _lastRecivedContext.AnimationController.Walking();
        }

        protected override void FixedDoImplementation()
        {
            var speed = _lastRecivedContext.Speed;
            Debug.Log($"Speed: {speed}");
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }

        private void StartSearchAtSound()
        {
            parent.ChangeStates(EnemyStates.SearchingAtSound);
        }
    }
}
