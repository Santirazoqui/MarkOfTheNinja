using Assets.Scripts.Enemy.Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class DetectedState : State
    {
        public float persectutionSpeed = 300f;
        public float minDistance = 0.1f;


        private Pathfinder pathfinder;
        private EnemyController parent;
        private GameObject player;
        
        private readonly string _playerTag = "Player";

        public override void CollitionEnter(Collision2D collision)
        {
            HandlePlayerCollition(collision);
        }

        private void HandlePlayerCollition(Collision2D collision)
        {
            bool collidedWithPlayer = collision.gameObject.CompareTag(_playerTag);
            if (!collidedWithPlayer) return;
            KillPlayer(collision.gameObject);
            parent.ChangeStates(EnemyController.EnemyStates.Chilling);
        }

        private void KillPlayer(GameObject player)
        {
            player.SetActive(false);
        }


        protected override void EnterImplementation()
        {
            pathfinder = _lastRecivedContext.Pathfinder;
            parent = _lastRecivedContext.Parent;
            player = _lastRecivedContext.Player;
        }

        protected override void FixedDoImplementation()
        {
            pathfinder.SetDestination(player.transform.position, DoNothing);
            var speed = persectutionSpeed;
            var minDistance = this.minDistance;
            pathfinder.AdjustPosition(speed,minDistance);
        }

        private void DoNothing() { }
    }
}
