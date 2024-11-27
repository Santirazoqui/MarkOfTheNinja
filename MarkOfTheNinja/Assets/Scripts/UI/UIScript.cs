using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class UIScript : MonoBehaviour
{
    private LevelManagerController levelManagerController;
    private void Awake()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
    }
    public void ResetLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ResetLastCheckpoint()
    {
        levelManagerController.RestartLevel();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
