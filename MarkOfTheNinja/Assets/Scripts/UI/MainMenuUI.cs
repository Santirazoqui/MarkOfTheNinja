using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Canvas SettingsCanvas;
    [SerializeField] Canvas LevelSelectCanvas;
    [SerializeField] AudioClip ButtonClickSound;
    [SerializeField] AudioSource audioSource;

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

    public void OpenLevelSelect()
    {
        this.gameObject.SetActive(false);
        LevelSelectCanvas.gameObject.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        LevelSelectCanvas.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void PlayHardLevel()
    {
        PlayButtonSound();
        SceneManager.LoadSceneAsync("BigLevel");
    }

    public void PlayTutorial()
    {
        PlayButtonSound();
        SceneManager.LoadSceneAsync("Tutorial");
    }
}
