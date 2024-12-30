using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class AlertEyeScript : MonoBehaviour
{
    [SerializeField] List<Sprite> alertEyes;
    public float secondsForEachFrame = 0.2f;
    private LevelManagerController levelManagerController;
    private IEnumerator closeOpenEyeRutine=null;

    Image alertEye;

    void Start()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        alertEye = GetComponent<Image>();
        levelManagerController.DetectedModeStarted += OpenEye;
        levelManagerController.DetectedModeEnded += CloseEye;
        levelManagerController.LevelWasReset += () => { alertEye.sprite = alertEyes[0]; };
    }
    private void OpenEye()
    {
        if (closeOpenEyeRutine != null) StopCoroutine(closeOpenEyeRutine);
        closeOpenEyeRutine = OpenOrCloseEye(false);
        StartCoroutine(closeOpenEyeRutine);
    }

    private void CloseEye()
    {
        if (!levelManagerController.PlayerAlive) return;
        if (closeOpenEyeRutine != null) StopCoroutine(closeOpenEyeRutine);
        closeOpenEyeRutine = OpenOrCloseEye(true);
        StartCoroutine(closeOpenEyeRutine);
    }
    private IEnumerator OpenOrCloseEye(bool close)
    {
        if (close)
        {
            for (int i = alertEyes.Count - 2; i >= 0; i--)
            {
                alertEye.sprite = alertEyes[i];
                yield return new WaitForSeconds(secondsForEachFrame);
            }
        }
        else
        {
            for (int i = 1; i < alertEyes.Count; i++)
            {
                alertEye.sprite = alertEyes[i];
                yield return new WaitForSeconds(secondsForEachFrame);
            }
        }
    }
}
