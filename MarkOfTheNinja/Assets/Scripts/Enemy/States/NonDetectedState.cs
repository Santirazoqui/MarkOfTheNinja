using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class NonDetectedState:State
    {
        public float searchingAtSoundSpeed = 400f;
        public float searchingSpeed = 300f;
        public override void PlayerIsBeingSeen(float distance)
        {
            _lastRecivedContext.LevelManagerController.PlayerIsBeingSeen(distance);
            _lastRecivedContext.Speed = searchingSpeed; 
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.Confused,_lastRecivedContext);
        }

        public override void TriggerEnter(Collider2D collision)
        {
            HandleSoundCollition(collision);
        }

        private void HandleSoundCollition(Collider2D collision)
        {
            if (!Util.Util.CollidedWithSound(_lastRecivedContext.Parent.gameObject, collision)) return;
            Debug.Log("Collided with sound");
            var soundOrigin = collision.gameObject.transform.position;
            levelManagerController.SoundWasHeard();
            _lastRecivedContext.SoundPosition = soundOrigin;
            _lastRecivedContext.Speed = searchingAtSoundSpeed;
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.GoingAtSound, _lastRecivedContext);
        }

    }
}
