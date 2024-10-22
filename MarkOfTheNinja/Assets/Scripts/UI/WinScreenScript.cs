using Assets.Scripts.DataAccess;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class WinScreenScript : MonoBehaviour
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
        text.text = $"Raw score: {gameData.Score.RawPoints} \nFast bonus: {gameData.Score.TimeBonus} \nTotal: {gameData.Score.Total} \nHigh score: {gameData.HighScore}";

    }
}
