using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.Enemy.EnemyAnimationController;

namespace Assets.Scripts.Enemy
{
    public class EnemyAnimationController : MonoBehaviour, IEnemyAnimationController
    {
        private Animator animator;
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

        private readonly Dictionary<AnimationStates, string> mapper = new() {
            { AnimationStates.Chilling, "Chilling" },
            { AnimationStates.Moving, "Moving" },
            { AnimationStates.Still, "Still" },
            { AnimationStates.Killing, "Kill"},
            { AnimationStates.Confused, "Confused"},
            { AnimationStates.SearchingAtSound, "Searching" },
            {AnimationStates.ThrowingFireball,"ThrowingFireball" }
        };


        private void Start()
        {
            animator = GetComponent<Animator>();
        }


        public void Walking()
        {
            EnterChilling();
            SetAnimationState(AnimationStates.Moving);
        }

        public void AlertWalking()
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
            SetAnimationState(AnimationStates.Confused);
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
                if (s == state){
                    animator.SetTrigger(n);
                }
                else if (state != AnimationStates.Chilling) animator.ResetTrigger(n);
            }
        }

        private void EnterChilling() => animator.SetBool(mapper[AnimationStates.Chilling], true);
        private void ExitChilling() => animator.SetBool(mapper[AnimationStates.Chilling], false);


    }


    public interface IEnemyAnimationController
    {
        public void Walking();

        public void AlertWalking();

        public void StayStill();

        public void Killing();

        public void Confused();

        public void SearchAtSound();

        public void ThrowFireball();
    }
}
