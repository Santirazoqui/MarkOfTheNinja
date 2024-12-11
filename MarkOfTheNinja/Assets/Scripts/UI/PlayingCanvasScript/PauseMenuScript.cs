using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    void Start()
    {
        
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        AudioListener.pause = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1;
    }
}
