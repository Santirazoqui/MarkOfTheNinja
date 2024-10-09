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
        public override void PlayerIsBeingSeen(float distance)
        {
            _lastRecivedContext.LevelManagerController.PlayerIsBeingSeen(distance);
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.Confused);
        }

        public override void TriggerEnter(Collider2D collision)
        {
            HandleSoundCollition(collision);
        }

        private void HandleSoundCollition(Collider2D collision)
        {
            if (!Util.CollidedWithSound(_lastRecivedContext.Parent.gameObject, collision)) return;
            var soundOrigin = collision.gameObject.transform.position;
            _lastRecivedContext.SoundPosition = soundOrigin;
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.GoingAtSound, _lastRecivedContext);
        }
    }
}
