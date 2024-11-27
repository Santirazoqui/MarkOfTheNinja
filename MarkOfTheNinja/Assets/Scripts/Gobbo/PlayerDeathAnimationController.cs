using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerDeathAnimationController:MonoBehaviour
    {
        public Animator animator;
        private enum AnimationStates
        {
            StaticDeath,
            KickDeath,
            Idle
        }

        private readonly Dictionary<AnimationStates, string> mapper = new() {
            { AnimationStates.KickDeath, "Kick_Death" },
            { AnimationStates.StaticDeath, "Gobbo_dead" },
            {AnimationStates.Idle, "Idle" }
        };


        public void KillPlayer() => SetAnimationState(AnimationStates.KickDeath);
        public void Idle() => SetAnimationState(AnimationStates.Idle);
        public void DeadOnTheGround() => SetAnimationState(AnimationStates.StaticDeath);
        private void SetAnimationState(AnimationStates state)
        {
            foreach (var (s, n) in mapper)
            {
                if (s == state) animator.SetTrigger(n);
                else animator.ResetTrigger(n);
            }
        }
    }
}
