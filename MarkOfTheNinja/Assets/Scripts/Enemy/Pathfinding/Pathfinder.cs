
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

        Rigidbody2D myRigidbody;
        private Action onReached;
        private bool tourchingBorder;
        private readonly string _wallsLayer = "Walls";
        private readonly string _enemyWalls = "EnemyWall";
        private Vector2 _previusPosition;
        private Vector2 _previusVelocity;
        public void SetDestination(Vector2 target, Action onReached)
        {
            targetPosition = target;
            this.onReached = onReached;
            //Debug.Log("Position Reajusted");
        }

        public void AdjustPosition(float speed, float minDistance)
        {
            ReajustPosition(speed, minDistance);
            ChangeCharacterOrientationDependingOnVelocity();
        }

        private void Start()
        {
            myRigidbody = GetComponent<Rigidbody2D>();
            targetPosition = myRigidbody.position;
        }

        private void ChangeCharacterOrientationDependingOnVelocity()
        {
            if (AnyOfTheGlobalsAreNull()) return;
            bool playerHasHorizontalSpedd = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
            if(playerHasHorizontalSpedd)
            {
                transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), transform.localScale.y);
            }
            /*bool lookingRight = rb.velocity.x >= 0.1f;
            bool lookingLeft = rb.velocity.x <= -0.1f;
            if (lookingRight)
            {
                transform.localScale.x = 1;
            }
            else if (lookingLeft)
            {
                transform.localScale.x = -1;
            }*/
        }

        private void ReajustPosition(float speed, float minDistance)
        {
            if (AnyOfTheGlobalsAreNull()) return;

            float distance = targetPosition.x - myRigidbody.position.x;
            bool closeEnough = Math.Abs(distance) < minDistance;
            if (closeEnough) {
                ResetVelocity();
                onReached();
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
            UnstuckingMechanism();
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
            return atTheSamePlaceThatWeWereAFrameAgo && sameVelocity;
        }

        private void CollisionLogic()
        {
            var collider = GetComponentInParent<BoxCollider2D>();
            if (collider.IsTouchingLayers(LayerMask.GetMask(_wallsLayer))
                || collider.IsTouchingLayers(LayerMask.GetMask(_enemyWalls)))
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

    public interface IPathfinder
    {
        void SetDestination(Vector2 destination, Action onReached);
        void AdjustPosition(float speed, float minDistance);
    }
}
