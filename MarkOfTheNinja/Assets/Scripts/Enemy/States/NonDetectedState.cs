using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class NonDetectedState:State
    {
        public float cooldownAfterPlayerLeavesFOV = 1f;
        private Vector2 lastPlayerPosition;
        public override void PlayerIsBeingSeen()
        {
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.Confused);
        }

        public override void PlayerLeftVisionRadius(Vector2 lastPlayerPosition)
        {
            this.lastPlayerPosition = lastPlayerPosition;
            Invoke(nameof(GoAtLastKnownLocation), cooldownAfterPlayerLeavesFOV);
        }

        public void GoAtLastKnownLocation()
        {
            _lastRecivedContext.SoundPosition = lastPlayerPosition;
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.GoingAtSound, _lastRecivedContext);
        }
    }
}
