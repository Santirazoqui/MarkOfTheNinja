using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    private LevelManagerController levelManagerController;
    private TextMeshProUGUI text;
    void Start()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = $"Puntaje: {levelManagerController.Score}";

    }
}
