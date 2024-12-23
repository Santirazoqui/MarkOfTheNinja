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
        PlayButtonSound();
        this.gameObject.SetActive(false);
        LevelSelectCanvas.gameObject.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        PlayButtonSound();
        LevelSelectCanvas.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void PlayEasyLevel()
    {
        PlayButtonSound();
        SceneManager.LoadSceneAsync("EasyLevel");
    }

    public void PlayMediumLevel()
    {
        PlayButtonSound();
        SceneManager.LoadSceneAsync("MediumLevel");
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
