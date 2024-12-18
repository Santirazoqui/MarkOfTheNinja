using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy.States.Detected
{
    public class ProximityDetected : MonoBehaviour
    {
        private bool detected =false;
        public bool DetectedMode
        {
            get => detected; set
            {
                detected = value;
            }
        }
        private CircleCollider2D collider;
        private readonly string enemyTag = "Enemy";
        // Use this for initialization
        void Start()
        {
            collider = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            InfectOtherEnemies(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            InfectOtherEnemies(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            InfectOtherEnemies(collision);
        }

        private void InfectOtherEnemies(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(enemyTag) && DetectedMode)
            {
                var enemyDetectionController = collision.gameObject.GetComponent<DetectionRateManager>();
                enemyDetectionController.EnemyWasInfectedWithDetected();
            }
        }
    }
}