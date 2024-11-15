using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenScript : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
