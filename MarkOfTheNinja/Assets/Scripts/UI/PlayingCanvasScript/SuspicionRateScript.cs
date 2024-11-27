using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class SuspicionRateScript : MonoBehaviour
{
    public int flashingRate = 2;
    // Start is called before the first frame update
    private LevelManagerController levelManagerController;
    private TextMeshProUGUI text;
    void Start()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        levelManagerController.PlayerWasDetected += PlayerWasDetected;
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
        int digitsAfterComma = 1;
        var rounder = Math.Pow(10, digitsAfterComma);
        var number = Math.Floor(levelManagerController.DetectionRate * rounder) / rounder;
        text.text = $"Suspicion: {Util.ChangeCommaToPoint(number+ "")}%";

    }
    private void UpdateTextColor()
    {
        if (!levelManagerController.Detected)
        {
            text.color = Color.Lerp(Color.green, Color.red, levelManagerController.DetectionRate / 100);
        }
    }

    private void PlayerWasDetected()
    {
        if(gameObject.activeInHierarchy) 
            StartCoroutine(FlashingColors());
    }

    private IEnumerator FlashingColors()
    {
        while (levelManagerController.Detected)
        {
            for (int i = 0; i < 100; i+=flashingRate)
            {
                text.color = Color.Lerp(Color.red, Color.white, i / 100f);
                yield return null;
            }
            for (int i = 0; i < 100; i+=flashingRate)
            {
                text.color = Color.Lerp(Color.white, Color.red, i / 100f);
                yield return null;
            }
        }
        yield break;
    }


}
