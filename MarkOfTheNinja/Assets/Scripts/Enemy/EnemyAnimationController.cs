using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyAnimationController : MonoBehaviour
    {
        public Animator animator;
        private Action enquedAction;
        private bool actionInQueue = false;
        private string targetAnimationName = "";
        public enum AnimationStates
        {
            Chilling,
            Killing
        }

        private readonly Dictionary<AnimationStates, string> mapper = new() {
            { AnimationStates.Chilling, "Chill" },
            { AnimationStates.Killing, "Kill"}
        };



        public void Walking()
        {
            SetAnimationState(AnimationStates.Chilling);
        }

        public void Killing(Action callBack)
        {
            SetAnimationState(AnimationStates.Killing);
            enquedAction = callBack;
            actionInQueue = true;
            targetAnimationName = "KillPlayerEnded";
        }

        private void SetAnimationState(AnimationStates state)
        {
            foreach (var (s, n) in mapper)
            {
                if (s == state) animator.SetTrigger(n);
                else animator.ResetTrigger(n);
            }
        }

        private void Update()
        {
            if (!actionInQueue) return;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(targetAnimationName))
            {
                actionInQueue = false;
                enquedAction();
            }
        }
    }
}
