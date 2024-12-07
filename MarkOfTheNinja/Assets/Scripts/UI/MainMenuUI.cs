using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Canvas SettingsCanvas;
    [SerializeField] AudioClip ButtonClickSound;
    [SerializeField] AudioSource audioSource;


    public void Play()
    {
        PlayButtonSound();
        SceneManager.LoadSceneAsync("Small Level 1");
    }

    public void Tutorial()
    {
        PlayButtonSound();
        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void OpenSettings()
    {
        PlayButtonSound();
        this.gameObject.SetActive(false);
        SettingsCanvas.gameObject.SetActive(true);
    }

    void PlayButtonSound()
    {
        audioSource.PlayOneShot(ButtonClickSound);
    }
}
