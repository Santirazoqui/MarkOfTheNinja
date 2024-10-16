using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy.States
{
    public class WaitingState:State
    {
        private EnemyStates prevState;
        private float waitTime;
        protected override void EnterImplementation()
        {
            prevState = _lastRecivedContext.WaitTime.Item2;
            waitTime = _lastRecivedContext.WaitTime.Item1;
            //Debug.Log($"Waiting for {waitTime}");
        }

        protected override void DoImplementation()
        {
            StartCoroutine(Exit());
        }

        private IEnumerator Exit()
        {
            yield return new WaitForSeconds( waitTime );
            //Debug.Log("Wait time ended");
            _lastRecivedContext.Parent.ChangeStates(prevState);
        }
    }
}
