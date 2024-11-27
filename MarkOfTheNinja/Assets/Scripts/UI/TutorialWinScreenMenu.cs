using Assets.Scripts.DataAccess;
using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class TutorialWinScreenMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private IDataAccessManager dataAccess;
    private TextMeshProUGUI text;
    private GameData gameData;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    [Inject]
    public void Contructor(IDataAccessManager manager)
    {
        dataAccess = manager;
        gameData = dataAccess.LoadData();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (dataAccess == null) return;
        var time = Math.Round((decimal)gameData.TimeSpentInLevel, 2);
        text.text = $"Time: {Util.ChangeCommaToPoint(time + "")}s";

    }
}
