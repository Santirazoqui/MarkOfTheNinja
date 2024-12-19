

namespace Assets.Scripts.Enemy.States
{
    public class SearchingAtSoundState:NonDetectedState
    {

        public string animationEndedEventName = "searchAnimationEnded";
        protected override void EnterImplementation()
        {
            animationController.SearchAtSound();
            pathfinder.AdjustPosition(0, 0);
            pathfinder.Waiting = true;
        }

        public override void AnimationEventFired(string eventDescription)
        {
            base.AnimationEventFired(eventDescription);
            parent.ChangeStates(EnemyStates.Chilling);
        }

        protected override void ExitImplementation()
        {
            pathfinder.Waiting = false;
        }
    }
}
