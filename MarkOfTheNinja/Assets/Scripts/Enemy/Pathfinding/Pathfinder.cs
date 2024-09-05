﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.Pathfinding
{
    public class Pathfinder:MonoBehaviour
    {
        public Vector2 targetPosition;

        Rigidbody2D rb;


        public void SetDestination(Vector3 target)
        {
            targetPosition = target;
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
            
            float distance = targetPosition.x - rb.position.x;
            bool closeEnough = Math.Abs(distance) < minDistance;
            if (closeEnough) {
                rb.velocity = new Vector2(0, rb.velocity.y);
                return;
            }
            
            float direction = Math.Sign(distance);
            float velocity = speed * Time.deltaTime * direction;

            rb.velocity = new Vector2(velocity, rb.velocity.y);
        }


    }
}
