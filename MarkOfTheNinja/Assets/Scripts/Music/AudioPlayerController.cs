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
    private bool detected = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayNonDetectedMusic();
    }


    private void PlayNonDetectedMusic()
    {
        audioSource.clip = nonDetectedMusic;
        audioSource.Play();
        audioSource.loop = true;
    }

    public void PlayDetectedMusic()
    {
        Debug.Log("Play detected music on start");
        audioSource.clip = detectedMusicStart;
        audioSource.Play();
        audioSource.loop = false;
        SubscribeToOnUpdate(CheckIfFirstDetectMusicEnded);
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
