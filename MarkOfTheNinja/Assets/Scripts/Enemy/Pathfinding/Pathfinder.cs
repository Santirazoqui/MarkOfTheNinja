
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.Pathfinding
{
    public class Pathfinder:MonoBehaviour, IPathfinder
    {
        public Vector2 targetPosition;
        public bool Waiting { get; set; } = false;
        Rigidbody2D myRigidbody;
        private Action onReached;
        private bool tourchingBorder;
        private readonly string _wallsLayer = "Walls";
        private readonly string _enemyWalls = "EnemyWall";
        private Vector2 _previusPosition;
        private Vector2 _previusVelocity;
        private float currentSpeed;
        private PathfinderMemory memory;
        public void SetDestination(Vector2 target, Action onReached)
        {
            targetPosition = target;
            this.onReached = onReached;
        }

        public void AdjustPosition(float speed, float minDistance)
        {
            if (AnyOfTheGlobalsAreNull()) return;
            currentSpeed = speed;
            ReajustPosition(speed, minDistance);
            ChangeCharacterOrientationDependingOnVelocity();
            UnstuckingMechanism();
        }

        public void TakeSnapshot()
        {
            memory = new()
            {
                Position = transform.position,
                LocalScale = transform.localScale,
                target = targetPosition,
                onReached = onReached
            };
        }

        public void ResetToSnapshot()
        {
            transform.position = memory.Position;
            transform.localScale = memory.LocalScale;
            targetPosition = memory.target;
            onReached = memory.onReached;
        }

        private void Start()
        {
            myRigidbody = GetComponent<Rigidbody2D>();
            targetPosition = myRigidbody.position;
        }

        private void ChangeCharacterOrientationDependingOnVelocity()
        {
            bool playerHasHorizontalSpedd = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
            if(playerHasHorizontalSpedd)
            {
                transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x) * Math.Abs(transform.localScale.x), transform.localScale.y);
            }
        }

        private void ReajustPosition(float speed, float minDistance)
        {

            float distance = targetPosition.x - myRigidbody.position.x;
            bool closeEnough = Math.Abs(distance) < minDistance;
            if (closeEnough) {
                ResetVelocity();
                onReached?.Invoke();
                return;
            }
            
            float direction = Math.Sign(distance);
            float velocity = speed * Time.deltaTime * direction;

            myRigidbody.velocity = new Vector2(velocity, myRigidbody.velocity.y);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionLogic();
        }

        private void FixedUpdate()
        {
            //UnstuckingMechanism();
        }

        private void UnstuckingMechanism()
        {
            if (Stuck())
            {
                onReached();
            }
            _previusPosition = myRigidbody.position;
            _previusVelocity = myRigidbody.velocity;
        }

        private bool Stuck()
        {
            bool atTheSamePlaceThatWeWereAFrameAgo = _previusPosition.x == myRigidbody.position.x;
            bool sameVelocity = _previusVelocity == myRigidbody.velocity;
            bool speedIsNotCero = currentSpeed != 0;
            return atTheSamePlaceThatWeWereAFrameAgo && sameVelocity && speedIsNotCero && !Waiting;
        }

        private void CollisionLogic()
        {
            var collider = GetComponentInParent<BoxCollider2D>();
            if (collider.IsTouchingLayers(LayerMask.GetMask(_enemyWalls)))
            {
                onReached();
            }
        }

        // Creo que esto pasa por un tema de concurrencia entre la inicializacion de todos los objetos
        private bool AnyOfTheGlobalsAreNull()
        {
            return targetPosition == null || myRigidbody == null;
        }

        private void ResetVelocity()
        {
            myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        }

        

    }
    public class PathfinderMemory
    {
        public Vector2 Position;
        public Vector3 LocalScale;
        public Vector2 target;
        public Action onReached;
    }
    public interface IPathfinder
    {
        public bool Waiting { get; set; }
        void SetDestination(Vector2 destination, Action onReached);
        public void TakeSnapshot();
        public void ResetToSnapshot();
        void AdjustPosition(float speed, float minDistance);
    }
}
