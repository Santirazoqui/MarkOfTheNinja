using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuspicionRateScript : MonoBehaviour
{
    // Start is called before the first frame update
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
        UpdateTextColor();
    }

    private void UpdateText()
    {
        text.text = $"Sospecha: {Math.Round(levelManagerController.DetectionRate)}%";
    }
    private void UpdateTextColor()
    {
        text.color = Color.Lerp(Color.green, Color.red, levelManagerController.DetectionRate/100);
    }


}
