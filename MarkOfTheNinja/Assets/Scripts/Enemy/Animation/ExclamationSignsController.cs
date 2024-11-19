using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.Animation
{
    public class ExclamationSignsController:MonoBehaviour
    {
        private Animator animator;
        private enum AnimationStates
        {
            Nothing,
            Exclamation,
            Question,
        }

        private readonly Dictionary<AnimationStates, string> mapper = new() {
            {AnimationStates.Nothing, "Nothing"},
            {AnimationStates.Exclamation, "Double_Exclamation"},
            {AnimationStates.Question, "Question_Mark"},
        };


        public void NoSign() => SetAnimationState(AnimationStates.Nothing);
        public void ExclamationSign() => SetAnimationState(AnimationStates.Exclamation);
        public void QuestionSign() => SetAnimationState(AnimationStates.Question);


        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        private void Update()
        {
            transform.localScale = new Vector2(-Math.Abs(transform.localScale.x), transform.localScale.y);
        }

        private void SetAnimationState(AnimationStates state)
        {
            foreach (var (s, n) in mapper)
            {
                if (s == state) animator?.SetTrigger(n);
                else animator?.ResetTrigger(n);
            }
        }
    }
}
