using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.Pathfinding
{
    public class EnemyBorderController:MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                var pathfinder = other.gameObject.GetComponentInChildren<Pathfinder>();
                pathfinder.OnBorderTrigger();
            }
        }
    }
}
