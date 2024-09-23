using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Enemy.States
{
    public class SearchingAtSoundState:NonDetectedState
    {

        public string animationEndedEventName = "searchAnimationEnded";
        protected override void EnterImplementation()
        {
            _lastRecivedContext.AnimationController.SearchAtSound();
            _lastRecivedContext.Pathfinder.AdjustPosition(0, 0);
        }

        public override void AnimationEventFired(string eventDescription)
        {
            _lastRecivedContext.Parent.ChangeStates(EnemyStates.Chilling);
        }
    }
}
