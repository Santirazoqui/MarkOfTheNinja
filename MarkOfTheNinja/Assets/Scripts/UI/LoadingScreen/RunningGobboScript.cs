using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.LoadingScreen
{
    public class RunningGobboScript:MonoBehaviour
    {
        private PlayerAnimationController animationController;
        private Animator animator;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            animationController = new PlayerAnimationController(animator);
            animationController.Running();
        }
        private void Update()
        {
            animationController.Running();
        }

    }
}
