using Assets.Scripts.Enemy.Pathfinding;
using System;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class DetectedState : State
    {
        public float persectutionSpeed = 300f;
        public float minDistance = 0.1f;


        private IPathfinder pathfinder;
        private EnemyController parent;
        private GameObject player;
        private ILevelManager levelManagerController;
        private EnemyAnimationController animationController;
        
        private readonly string _playerTag = "Player";

        public override void CollitionEnter(Collision2D collision)
        {
            HandlePlayerCollition(collision.gameObject);
        }

        private void HandlePlayerCollition(GameObject player)
        {
            bool collidedWithPlayer = player.CompareTag(_playerTag);
            if (!collidedWithPlayer) return;
            KillPlayer(player.gameObject);
        }

        private void KillPlayer(GameObject player)
        {
            animationController.Killing(PostKilling);
            parent.ChangeStates(EnemyStates.StayingStill);
        }

        private void PostKilling()
        {
            player.SetActive(false);
            levelManagerController.PublishEnemyStateChange(EnemyStates.Chilling);
        }


        protected override void EnterImplementation()
        {
            pathfinder = _lastRecivedContext.Pathfinder;
            parent = _lastRecivedContext.Parent;
            player = _lastRecivedContext.Player;
            levelManagerController = _lastRecivedContext.LevelManagerController;
            animationController = _lastRecivedContext.AnimationController;
            PlayDetectedAnimation();
        }

        private void PlayDetectedAnimation()
        {
            animationController.Walking();
        }

        protected override void FixedDoImplementation()
        {
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
