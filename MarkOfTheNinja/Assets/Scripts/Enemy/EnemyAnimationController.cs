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
        public enum AnimationStates
        {
            Chilling,
            Killing,
            Confused,
            SearchingAtSound
        }

        private readonly Dictionary<AnimationStates, string> mapper = new() {
            { AnimationStates.Chilling, "Chill" },
            { AnimationStates.Killing, "Kill"},
            { AnimationStates.Confused, "Confused"},
            { AnimationStates.SearchingAtSound, "Searching" }
        };



        public void Walking()
        {
            SetAnimationState(AnimationStates.Chilling);
        }

        public void Killing()
        {
            SetAnimationState(AnimationStates.Killing);
        }

        public void Confused()
        {
            SetAnimationState(AnimationStates.Confused);
        }

        public void SearchAtSound()
        {
            SetAnimationState(AnimationStates.SearchingAtSound);
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
        }
    }
}
