using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRateManager : MonoBehaviour
{

    public bool Detected { get; private set; }
    public float DetectionRate { get; private set; } = 0;

    [Header("Detection Rate")]
    public float visualDetectionRate = 100f;
    public float expoentialDistanceMultiplier = 2f;
    public float audioDetectionRate = 50f;
    [Header("Detection Rate Reduction")]
    public float secondsBeforeDetectionDecreases = 2f;
    public float minDecreasePerMilisecond = 0.1f;
    public float maxDecreasePerMilisecond = 0.2f;
    public float increaseInDecreasePerMilisecond = 0.001f;

    private LevelManagerController levelManagerController; 
    private EnemyController enemyController;
    private IEnumerator previousDetectionDecresionRoutine = null;
    // Start is called before the first frame update
    void Start()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        levelManagerController.LevelWasReset += OnReset;
        enemyController = GetComponentInParent<EnemyController>();  
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


    private void OnReset()
    {
        StopDetectionDecreasion();
        DetectionRate = 0;
        Detected = false;
    }
    public void EnemyWasInfectedWithDetected()
    {
        if (Detected) return;
        Detected = true;
        DetectionRate = 100;
        enemyController.ChangeStates(EnemyStates.DetectedPatrolling);
        StopDetectionDecreasion();
    }

    public void PlayerWasInstaDetected()
    {
        DetectionRate = 100;
        EnterDetectedPhase();
    }

    public void LeftDetectedMode()
    {
        StopDetectionDecreasion();
        DetectionRate = 0;
        Detected = false;
    }

    private void PlayerWasPerceived(float detectionRate, float multiplier = 1)
    {
        if (Detected) return;
        DetectionRate += detectionRate * multiplier * Time.deltaTime;
        StartDetectionDecrease();
        if (DetectionRate >= 100)
        {
            DetectionRate = 100;
            Detected = true;
            EnterDetectedPhase();
        }
    }

    private void EnterDetectedPhase()
    {
        //levelManagerController.PlayerWasInstaDetected();
        enemyController.ChangeStates(EnemyStates.DetectedPatrolling);
        levelManagerController.EnterDetectedPhase();
        StopDetectionDecreasion();
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
        float decrease = minDecreasePerMilisecond;
        while (DetectionRate > 0)
        {
            DetectionRate -= decrease;

            if (decrease < maxDecreasePerMilisecond)
            {
                decrease += increaseInDecreasePerMilisecond;
            }
            else
            {
                decrease = maxDecreasePerMilisecond;
            }
            yield return new WaitForSeconds(0.001f);
        }
        DetectionRate = 0;
        yield break;
    }

    private void StopDetectionDecreasion()
    {
        if (previousDetectionDecresionRoutine is not null) StopCoroutine(previousDetectionDecresionRoutine);
    }

}
