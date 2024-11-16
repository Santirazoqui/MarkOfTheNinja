using Assets.Scripts.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ButtonInWinScreenScript : MonoBehaviour
{
    private IDataAccessManager _dataAccessManager;
    private ISceneSwitcher _sceneSwitcher;
    [Inject]
    public void Constructor(IDataAccessManager dataAccess, ISceneSwitcher sceneSwitcher)
    {
        _dataAccessManager = dataAccess;
        _sceneSwitcher = sceneSwitcher;
    }
    public void GoToLevel()
    {
        var gameData = _dataAccessManager.LoadData();
        _sceneSwitcher.ChangeScenes(gameData.GameSceneIndex ?? throw new Exception("Game Scene Index was null"));
    }
}
