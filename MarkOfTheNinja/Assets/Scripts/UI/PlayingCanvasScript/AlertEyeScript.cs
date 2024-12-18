using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class AlertEyeScript : MonoBehaviour
{
    [SerializeField] List<Sprite> alertEyes;

    private LevelManagerController levelManagerController;

    Image alertEye;

    void Start()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        alertEye = GetComponent<Image>();
    }

    void Update()
    {
        ChangeAlertEye();
    }

    void ChangeAlertEye()
    {
        int digitsAfterComma = 1;
        var rounder = Math.Pow(10, digitsAfterComma);
        var number = Math.Floor(0 * rounder) / rounder;
        if(number == 0)
        {
            alertEye.sprite = alertEyes[0];
        }
        else if(number > 0 && number <= 25)
        {
            alertEye.sprite = alertEyes[1];
        }
        else if(number > 25 && number <= 50)
        {
            alertEye.sprite = alertEyes[2];
        }
        else if(number > 50 && number <= 75)
        {
            alertEye.sprite = alertEyes[3];
        }
        else if(number > 75 && number < 100)
        {
            alertEye.sprite = alertEyes[4];
        } 
        else if(number == 100)
        {
            alertEye.sprite = alertEyes[5];
        }
    }
}
