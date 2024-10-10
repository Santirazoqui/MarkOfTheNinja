using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class LevelManagerController : MonoBehaviour, ILevelManager
{
    public delegate void OnGlobalEnemyStateChange(EnemyStates state);
    public event OnGlobalEnemyStateChange StateChanged;
    public float DetectionRate { get; private set; } = 0;
    public bool Detected { get; private set; }

    public float visualDetectionRate = 100f;
    public float audioDetectionRate = 50f;
    public float EnemySuspicionPercentage {  get; private set; }

    public void PublishEnemyStateChange(EnemyStates state)
    {
        StateChanged?.Invoke(state);
    }
    
    public void PlayerWasInstaDetected()
    {
        DetectionRate = 100;
        EnterDetectedPhase();
    }

    public void PlayerIsBeingSeen(float distance)
    {
        var multiplier = distance != 0 ? 1 / distance : 1;
        PlayerWasPerceived(visualDetectionRate, multiplier);
    }

    public void SoundWasHeard()
    {
        PlayerWasPerceived(audioDetectionRate);
    }

    private void PlayerWasPerceived(float detectionRate, float multiplier = 1)
    {
        if (Detected) return;
        DetectionRate += detectionRate * multiplier * Time.deltaTime;
        if (DetectionRate >= 100)
        {
            DetectionRate = 100;
            EnterDetectedPhase();
        }
        Debug.Log("Detection rate:" + DetectionRate);
    }

    private void EnterDetectedPhase()
    {
        Detected = true;
        PublishEnemyStateChange(EnemyStates.Detected);
    }
}

public interface ILevelManager
{
    void PlayerIsBeingSeen(float distance);
    void SoundWasHeard();
    void PublishEnemyStateChange(EnemyStates state);
}
