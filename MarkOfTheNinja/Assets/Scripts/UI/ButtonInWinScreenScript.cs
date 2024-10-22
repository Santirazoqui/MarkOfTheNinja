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
    [Inject]
    public void Constructor(IDataAccessManager dataAccess)
    {
        _dataAccessManager = dataAccess;
    }
    public void GoToLevel()
    {
        var gameData = _dataAccessManager.LoadData();
        SceneManager.LoadScene(gameData.GameSceneIndex ?? throw new Exception("Game Scene Index was null"));
    }
}
