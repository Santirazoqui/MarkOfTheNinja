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
        protected override void EnterImplementation()
        {
            _lastRecivedContext.AnimationController.Confused();
            _lastRecivedContext.Pathfinder.AdjustPosition(0, 0);
        }

        public override void PlayerIsBeingSeen()
        {
        }
    }
}
