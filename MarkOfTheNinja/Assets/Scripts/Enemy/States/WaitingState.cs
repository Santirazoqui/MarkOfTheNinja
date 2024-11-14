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
            prevState = _lastRecivedContext.WaitTime.Item2;
            waitTime = _lastRecivedContext.WaitTime.Item1;
            animationController.StayStill();
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
            _lastRecivedContext.Parent.ChangeStates(prevState);
        }

        protected override void ExitImplementation()
        {
            StopCoroutine(exitRutine);
        }
    }
}
