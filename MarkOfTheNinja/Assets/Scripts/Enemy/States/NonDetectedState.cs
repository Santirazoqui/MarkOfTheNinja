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
        public override void PlayerIsBeingSeen()
        {
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.Confused);
        }


    }
}
