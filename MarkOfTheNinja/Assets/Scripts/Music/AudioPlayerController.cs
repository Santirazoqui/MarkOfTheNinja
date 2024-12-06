using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerController : SubscribeOnUpdate
{
    // Start is called before the first frame update
    public AudioClip nonDetectedMusic;
    public AudioClip detectedMusicStart;
    public AudioClip detectedMusicLoop;
    private AudioSource audioSource;
    private AudioSource audioSource2;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource2 = GetComponents<AudioSource>()[1]; //esto no es lo mejor
        PlayNonDetectedMusic();
    }


    public void PlayNonDetectedMusic()
    {
        audioSource.clip = nonDetectedMusic;
        audioSource.Play();
        audioSource.loop = true;
    }

    public void PlayDetectedMusic()
    {
        //Debug.Log("Play detected music on start");
        audioSource2.clip = detectedMusicStart;
        audioSource2.Play();
        audioSource2.loop = false;

        audioSource.clip = detectedMusicLoop;
        audioSource.Play();
        audioSource.loop = true;

        //SubscribeToOnUpdate(CheckIfFirstDetectMusicEnded);
    }

    private void CheckIfFirstDetectMusicEnded()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = detectedMusicLoop;
            audioSource.Play();
            audioSource.loop = true;
            UnsubscribeOfOnUpdate(CheckIfFirstDetectMusicEnded);
        }
    }
}
