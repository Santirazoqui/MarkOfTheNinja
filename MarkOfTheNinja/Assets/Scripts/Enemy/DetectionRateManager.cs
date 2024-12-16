using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRateManager : MonoBehaviour
{

    public delegate void OnDetection();
    public event OnDetection PlayerWasDetected;
    public bool Detected { get; private set; }
    public float DetectionRate { get; private set; } = 0;

    [Header("Detection Rate")]
    public float visualDetectionRate = 100f;
    public float expoentialDistanceMultiplier = 2f;
    public float audioDetectionRate = 50f;
    [Header("Detection Rate Reduction")]
    public float secondsBeforeDetectionDecreases = 2f;
    public float minDecreasePerTick = 0.1f;
    public float maxDecreasePerTick = 0.2f;
    public float increaseInDecreasePerTick = 0.001f;

    private LevelManagerController levelManagerController;
    private IEnumerator previousDetectionDecresionRoutine = null;
    // Start is called before the first frame update
    void Start()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerIsBeingSeen(float distance)
    {
        //Debug.Log("Player was seen");
        var multiplier = distance != 0 ? 1 / (float)(Math.Pow(distance, expoentialDistanceMultiplier)) : 1;
        PlayerWasPerceived(visualDetectionRate, multiplier);
    }
    public void SoundWasHeard()
    {
        //Debug.Log("Player was heard");
        PlayerWasPerceived(audioDetectionRate);
    }

    private void PlayerWasPerceived(float detectionRate, float multiplier = 1)
    {
        if (Detected) return;
        DetectionRate += detectionRate * multiplier * Time.deltaTime;
        StartDetectionDecrease();
        if (DetectionRate >= 100)
        {
            DetectionRate = 100;

            EnterDetectedPhase();
        }
    }

    private void EnterDetectedPhase()
    {
        PlayerWasDetected?.Invoke();
    }

    private void StartDetectionDecrease()
    {
        if (Detected) return;
        StopDetectionDecreasion();
        previousDetectionDecresionRoutine = StartSuspicionDecrease();
        StartCoroutine(previousDetectionDecresionRoutine);
    }

    private IEnumerator StartSuspicionDecrease()
    {
        yield return new WaitForSeconds(secondsBeforeDetectionDecreases);
        float decrease = minDecreasePerTick;
        while (DetectionRate > 0)
        {
            DetectionRate -= decrease;

            if (decrease < maxDecreasePerTick)
            {
                decrease += increaseInDecreasePerTick;
            }
            else
            {
                decrease = maxDecreasePerTick;
            }
            yield return new WaitForFixedUpdate();
        }
        DetectionRate = 0;
        yield break;
    }

    private void StopDetectionDecreasion()
    {
        if (previousDetectionDecresionRoutine is not null) StopCoroutine(previousDetectionDecresionRoutine);
    }

}
