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
        private ILevelManager levelManagerController;
        
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
            levelManagerController.PublishEnemyStateChange(EnemyStates.Chilling);
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
            levelManagerController = _lastRecivedContext.LevelManagerController;
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
