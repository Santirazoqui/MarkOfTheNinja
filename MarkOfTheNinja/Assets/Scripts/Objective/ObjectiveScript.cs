using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour
{
    Animator myAnimator;
    LevelManagerController levelManagerController;

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
        }
        this.levelManagerController.canWin = true;
    }
}
