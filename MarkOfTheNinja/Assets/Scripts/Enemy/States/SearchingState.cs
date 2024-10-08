﻿using Assets.Scripts.Enemy.Pathfinding;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class SearchingState : NonDetectedState
    {
        public float searchingSpeed = 300f;
        public float minDistance = 0.1f;


        private IPathfinder pathfinder;
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
            pathfinder.SetDestination(soundOrigin, StartSearchAtSound);
        }


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
            var speed = this.searchingSpeed;
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed, minDistance);
        }

        private void StartSearchAtSound()
        {
            parent.ChangeStates(EnemyStates.SearchingAtSound);
        }
    }
}
