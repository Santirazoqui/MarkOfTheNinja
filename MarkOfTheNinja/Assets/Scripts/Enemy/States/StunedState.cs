using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class StunedState:NonDetectedState
    {
        private EnemyStates prevState;
        protected override void EnterImplementation()
        {
            animationController.Stunned();
            pathfinder.AdjustPosition(0, 0);
            pathfinder.Waiting = true;
            //Debug.Log($"Waiting for {waitTime}");
        }

        public override void AnimationEventFired(string eventDescription)
        {
            base.AnimationEventFired(eventDescription);
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.Chilling);
        }

        protected override void ExitImplementation()
        {
            pathfinder.Waiting = false;
        }
    }
}
