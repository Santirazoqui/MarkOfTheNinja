using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class WaitingState:NonDetectedState
    {
        private EnemyStates prevState;
        private float waitTime;
        private IEnumerator exitRutine;
        protected override void EnterImplementation()
        {
            prevState = _lastRecivedContext.PreviousState;
            waitTime = _lastRecivedContext.WaitTime;
            animationController.StayStill();
            pathfinder.AdjustPosition(0, 0);
            pathfinder.Waiting = true;
            //Debug.Log($"Waiting for {waitTime}");
        }

        protected override void DoImplementation()
        {
            exitRutine = Exit();
            StartCoroutine(exitRutine);
        }

        private IEnumerator Exit()
        {
            yield return new WaitForSeconds( waitTime );
            //Debug.Log("Wait time ended");
            pathfinder.Waiting = false;
            _lastRecivedContext.Parent.ChangeStates(prevState);
        }

        protected override void ExitImplementation()
        {
            StopCoroutine(exitRutine);
        }
    }
}
