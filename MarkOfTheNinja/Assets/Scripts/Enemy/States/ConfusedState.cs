using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class ConfusedState:NonDetectedState
    {
        public float cooldownAfterPlayerLeavesFOV = 1f;
        private Vector2 lastPlayerPosition;
        protected override void EnterImplementation()
        {
            _lastRecivedContext.AnimationController.Confused();
            _lastRecivedContext.Pathfinder.AdjustPosition(0, 0);
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
