using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour
{
    Animator myAnimator;
    LevelManagerController levelManagerController;
    AudioSource[] audioSources;
    private bool wasOpened = false;

    [SerializeField] GameObject exitBlocker;
    [SerializeField] bool exitBlocked;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        audioSources = GetComponents<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !wasOpened)
        {
            //Debug.Log("Player entered the objective");
            myAnimator.SetBool("Open", true);
            if (this.exitBlocked)
            {
                //Debug.Log("Open Exit");
                this.exitBlocker.SetActive(false);
                //Debug.Log(exitBlocker);
            }
            this.levelManagerController.canWin = true;

            if (!wasOpened)
            {
                foreach (AudioSource audioSource in audioSources)
                {
                    audioSource.Play();
                }
            }

            wasOpened = true;
        }
    }
}
