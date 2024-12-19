using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy.Animation
{
    public class DetectionCircle : MonoBehaviour
    {
        private DetectionRateManager detectionRateManager;
        [SerializeField] List<Sprite> alertEyes;

        SpriteRenderer alertEye;

        // Use this for initialization
        void Start()
        {
            detectionRateManager = GetComponentInParent<DetectionRateManager>();
            alertEye = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            ChangeAlertEye();
        }

        void ChangeAlertEye()
        {
            int digitsAfterComma = 1;
            var rounder = Math.Pow(10, digitsAfterComma);
            var number = Math.Floor(detectionRateManager.DetectionRate * rounder) / rounder;
            if (number == 0)
            {
                alertEye.sprite = alertEyes[0];
            }
            else if (number > 0 && number <= 25)
            {
                alertEye.sprite = alertEyes[1];
            }
            else if (number > 25 && number <= 50)
            {
                alertEye.sprite = alertEyes[2];
            }
            else if (number > 50 && number <= 75)
            {
                alertEye.sprite = alertEyes[3];
            }
            else if (number > 75 && number < 100)
            {
                alertEye.sprite = alertEyes[4];
            }
            else if (number == 100)
            {
                alertEye.sprite = alertEyes[5];
            }
        }
    }
}