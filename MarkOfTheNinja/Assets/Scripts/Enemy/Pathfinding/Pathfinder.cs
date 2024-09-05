
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
        private float speed = 30f;

        Rigidbody2D rb;


        public void SetDestination(Vector3 target)
        {
            targetPosition = target;
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            targetPosition = rb.position;
        }

        private void FixedUpdate()
        {

            float direction = (targetPosition.x - rb.position.x);
            float velocity = speed * Time.deltaTime * direction;

            rb.velocity = new Vector2(velocity,rb.velocity.y);
            Debug.Log($"Added force: {velocity}");

        }
    }
}
