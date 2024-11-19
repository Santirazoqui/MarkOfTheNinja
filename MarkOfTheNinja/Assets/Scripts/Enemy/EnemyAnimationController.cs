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

        public GameObject enemyTopAnimations;

        public enum AnimationStates
        {
            Chilling,
            Moving,
            Still,
            Killing,
            Confused,
            SearchingAtSound,
            ThrowingFireball
        }

        public enum EnemyTopAnimations
        {
            Confused,
            Killing
        }

        private readonly Dictionary<AnimationStates, string> mapper = new() {
            { AnimationStates.Chilling, "Chilling" },
            { AnimationStates.Moving, "Moving" },
            { AnimationStates.Still, "Still" },
            { AnimationStates.Killing, "Kill"},
            { AnimationStates.Confused, "Confused"},
            { AnimationStates.SearchingAtSound, "Searching" },
            {AnimationStates.ThrowingFireball,"ThrowingFireball" }
        };



        private readonly Dictionary<EnemyTopAnimations, string> topAnimationsMapper = new()
        {
            {EnemyTopAnimations.Confused, "QuestionMark"},
            {EnemyTopAnimations.Killing, "ExclamationMark"}
        };


        void Start()
        {
            GameObject temp = Instantiate(enemyTopAnimations, transform.position, Quaternion.identity, transform);
            temp.transform.localPosition = new Vector3(0f, 0.31f, 0f);
        }

        public void Walking()
        {
            EnterChilling();
            SetAnimationState(AnimationStates.Moving);
        }

        public void StayStill()
        {
            EnterChilling();
            SetAnimationState(AnimationStates.Still);
        }

        public void Killing()
        {
            ExitChilling();
            SetAnimationState(AnimationStates.Killing);
        }

        public void Confused()
        {
            ExitChilling();
            //SetAnimationState(AnimationStates.Confused);
            PlayTopAnimation(EnemyTopAnimations.Confused);
        }

        public void SearchAtSound()
        {
            ExitChilling();
            SetAnimationState(AnimationStates.SearchingAtSound);
        }

        public void ThrowFireball()
        {
            ExitChilling();
            SetAnimationState(AnimationStates.ThrowingFireball);
        }



        private void SetAnimationState(AnimationStates state)
        {
            foreach (var (s, n) in mapper)
            {
                if (s == AnimationStates.Chilling) continue;
                if (s == state)
                {
                    animator.SetTrigger(n);
                }
                else if (state != AnimationStates.Chilling) animator.ResetTrigger(n);
            }
        }

        private void PlayTopAnimation(EnemyTopAnimations topAnimation)
        {
            Animator topAnimator = enemyTopAnimations.GetComponent<Animator>();
            string animationTrigger = topAnimationsMapper[topAnimation];
            //Debug.Log(animationTrigger);
            topAnimator.enabled = true;
            topAnimator.SetBool(animationTrigger, true);
        }

        private void EnterChilling() => animator.SetBool(mapper[AnimationStates.Chilling], true);
        private void ExitChilling() => animator.SetBool(mapper[AnimationStates.Chilling], false);


    }
}
