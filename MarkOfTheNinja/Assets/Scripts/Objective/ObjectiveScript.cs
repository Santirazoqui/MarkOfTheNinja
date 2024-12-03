using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour
{
    Animator myAnimator;
    LevelManagerController levelManagerController;

    [SerializeField] GameObject exitBlocker;
    [SerializeField] bool exitBlocked;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            myAnimator.SetBool("Open", true);
            if (this.exitBlocked)
            {
                this.exitBlocker.SetActive(false);
            }
            this.levelManagerController.canWin = true;
        }   
    }
}
