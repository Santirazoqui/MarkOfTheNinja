using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class NonDetectedState:State
    {
        public float searchingAtSoundSpeed = 400f;
        public float searchingSpeed = 300f;
        private readonly string stumpAnimationEvent = "stumpEnded";
        public override void PlayerIsBeingSeen(float distance)
        {
            detectionRateManager.PlayerIsBeingSeen(distance);
            _lastRecivedContext.Speed = searchingSpeed; 
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.Confused,_lastRecivedContext);
        }

        public override void TriggerEnter(Collider2D collision)
        {
            HandleSoundCollition(collision);
        }

        private void HandleSoundCollition(Collider2D collision)
        {
            if (!CollidedWithSound(_lastRecivedContext.Parent.gameObject, collision)) return;
            //Debug.Log("Collided with sound");
            var soundOrigin = collision.gameObject.transform.position;
            detectionRateManager.SoundWasHeard();
            _lastRecivedContext.SoundPosition = soundOrigin;
            _lastRecivedContext.Speed = searchingAtSoundSpeed;
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.GoingAtSound, _lastRecivedContext);
        }

        public override void AnimationEventFired(string eventDescription)
        {
            if (eventDescription == stumpAnimationEvent)
            {
                parent.ChangeStates(_lastRecivedContext.PreviousState);
            }
        }

        private bool CollidedWithSound(GameObject you, Collider2D collision)
        {
            bool collidedWithASound = collision.gameObject.CompareTag("Sound");
            if (!collidedWithASound) return false;
            var objective = collision.bounds.center;
            //An arbitrary offset to get the center of the sound off the ground
            objective.y += 0.5f;
            var enemyPosition = you.transform.position;
            //DrawPoints(objective, enemyPosition);
            if (ObjectDetector.AnyObjectsBetween(enemyPosition, objective)) return false;
            return true;
        }

        private void DrawPoints(Vector2 sound, Vector2 enemy)
        {
            var drawer = FindAnyObjectByType<PointGeneratorScript>();
            drawer.DrawPoint(enemy, Color.red);
            drawer.DrawPoint(sound, Color.green);
        }


    }
}
