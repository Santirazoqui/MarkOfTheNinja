using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimationController
    {
        private readonly Animator animator;
        private enum AnimationStates
        {
            Still,
            Running,
            Jumping,
            OnAir,
            Landing,
            Dashing,
            Dying,
        }

        private readonly Dictionary<AnimationStates, string> mapper = new() {
            { AnimationStates.Still, "Idling" },
            { AnimationStates.Running, "Walk Cycle" },
            { AnimationStates.Jumping, "Jumping" },
            { AnimationStates.OnAir, "OnAir"},
            { AnimationStates.Landing, "Landing"},
            { AnimationStates.Dashing, "Dash" },
            { AnimationStates.Dying, "Dying" },
        };

        public PlayerAnimationController(Animator animator)
        {  
            this.animator = animator; 
        }

        public void Dash() => SetAnimationState(AnimationStates.Dashing);

        public void Idle() => SetAnimationState(AnimationStates.Still);

        public void Running() => SetAnimationState(AnimationStates.Running);

        public void JumpStart() => SetAnimationState(AnimationStates.Jumping);

        public void OnAir() => SetAnimationState(AnimationStates.OnAir);

        public void Landed() => SetAnimationState(AnimationStates.Landing);

        public void Dying() => SetAnimationState(AnimationStates.Dying);

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
