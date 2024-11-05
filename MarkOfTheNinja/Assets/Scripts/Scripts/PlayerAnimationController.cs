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
        }

        private readonly Dictionary<AnimationStates, string> mapper = new() {
            { AnimationStates.Still, "idle" },
            { AnimationStates.Running, "running" },
            { AnimationStates.Jumping, "jumping" },
            { AnimationStates.OnAir, "onAir"},
            { AnimationStates.Landing, "landing"},
            { AnimationStates.Dashing, "dashing" },
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
