﻿
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

        Rigidbody2D rb;
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
            rb = GetComponent<Rigidbody2D>();
            targetPosition = rb.position;
        }

        private void ChangeCharacterOrientationDependingOnVelocity()
        {
            if (AnyOfTheGlobalsAreNull()) return;
            bool lookingRight = rb.velocity.x >= 0.1f;
            bool lookingLeft = rb.velocity.x <= -0.1f;
            if (lookingRight)
            {
                transform.localScale = Vector3.one;
            }
            else if (lookingLeft)
            {
                transform.localScale = new Vector3(-1,1,1);
            }
        }

        private void ReajustPosition(float speed, float minDistance)
        {
            if (AnyOfTheGlobalsAreNull()) return;

            float distance = targetPosition.x - rb.position.x;
            bool closeEnough = Math.Abs(distance) < minDistance;
            if (closeEnough) {
                ResetVelocity();
                onReached();
                return;
            }
            
            float direction = Math.Sign(distance);
            float velocity = speed * Time.deltaTime * direction;

            rb.velocity = new Vector2(velocity, rb.velocity.y);
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
            _previusPosition = rb.position;
            _previusVelocity = rb.velocity;
        }

        private bool Stuck()
        {
            bool atTheSamePlaceThatWeWereAFrameAgo = _previusPosition.x == rb.position.x;
            bool sameVelocity = _previusVelocity == rb.velocity;
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
            return targetPosition == null || rb == null;
        }

        private void ResetVelocity()
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }



    }

    public interface IPathfinder
    {
        void SetDestination(Vector2 destination, Action onReached);
        void AdjustPosition(float speed, float minDistance);
    }
}
