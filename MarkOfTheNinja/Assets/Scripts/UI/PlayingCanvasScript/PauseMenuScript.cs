using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private LevelManagerController levelManagerController;

    private void Awake()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
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

    public void ResetLevelFromPause()
    {
        Resume();
        ResetLevel(); 
    }

    public void GoToLastCheckpoint()
    {
        Resume();
        ResetLastCheckpoint();
    }

    public void Menu()
    {
        Resume();
        GoToMainMenu();
    }

    private void ResetLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void ResetLastCheckpoint()
    {
        levelManagerController.RestartLevel();
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
