using Assets.Scripts.DataAccess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Canvas SettingsCanvas;
    private IDataAccessManager dataAccessManager;

    [Inject]
    public void Constructor(IDataAccessManager dataAccessManager)
    {
        this.dataAccessManager = dataAccessManager;
        var gameData = dataAccessManager.LoadData();
        gameData.SpawnPoint = null;
        dataAccessManager.SaveData(gameData);
    }
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
