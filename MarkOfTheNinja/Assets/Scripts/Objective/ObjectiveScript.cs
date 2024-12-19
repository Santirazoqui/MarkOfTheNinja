using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ObjectiveScript : MonoBehaviour
{
    Animator myAnimator;
    LevelManagerController levelManagerController;
    AudioSource[] audioSources;
    private bool wasOpened = false;
    private Light2D light;
    [SerializeField] AudioClip gateOpenClip;

    [SerializeField] GameObject exitBlocker;
    [SerializeField] bool exitBlocked;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        audioSources = GetComponents<AudioSource>();
        light = GetComponentInChildren<Light2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !wasOpened)
        {
            //Debug.Log("Player entered the objective");
            light.intensity = 0;
            myAnimator.SetBool("Open", true);
            if (this.exitBlocked)
            {
                //Debug.Log("Open Exit");
                this.exitBlocker.SetActive(false);
                //Debug.Log(exitBlocker);
            }
            this.levelManagerController.CanWin = true;

            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.Play();
            }

            StartCoroutine(PlayGateOpenClip());
            wasOpened = true;
        }
    }

    IEnumerator PlayGateOpenClip()
    {
        //yield return new WaitForSeconds(audioSources[0].clip.length);
        yield return new WaitForSeconds(2);
        audioSources[0].clip = gateOpenClip;
        audioSources[0].Play();
    }
}
