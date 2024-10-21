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
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    [Inject]
    public void Contructor(IDataAccessManager manager)
    {
        dataAccess = manager;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (dataAccess == null) return;
        var gameData = dataAccess.LoadData();
        text.text = $"Score: {gameData.Score} \n High score: {gameData.HighScore}";

    }
}
