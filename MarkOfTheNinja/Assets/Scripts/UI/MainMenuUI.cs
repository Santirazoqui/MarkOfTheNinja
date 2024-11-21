using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Canvas SettingsCanvas;
	
	

    public void Play()
    {
        SceneManager.LoadSceneAsync("Small Level 1");
    }

    public void Tutorial()
    {
        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void OpenSettings()
    {
        this.gameObject.SetActive(false);
        SettingsCanvas.gameObject.SetActive(true);
    }
}
