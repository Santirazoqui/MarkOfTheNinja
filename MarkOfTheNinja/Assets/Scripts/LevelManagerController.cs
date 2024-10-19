using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LevelManagerController : SubscribeOnUpdate, ILevelManager
{
    public delegate void OnGlobalEnemyStateChange(EnemyStates state);
    public event OnGlobalEnemyStateChange StateChanged;

    public delegate void OnDetection();
    public event OnDetection PlayerWasDetected;
    public float DetectionRate { get; private set; } = 0;
    public bool Detected { get; private set; }

    public float visualDetectionRate = 100f;
    public float audioDetectionRate = 50f;
    public float globalLightMin = 0.1f;
    public float globalLightMax = 0.5f;
    private float lightIncrementsPerTick = 0.01f;
    public float EnemySuspicionPercentage {  get; private set; }

    private Light2D GlobalLight { get; set; }
    private AudioPlayerController AudioController { get; set; }

    private void Start()
    {
        GlobalLight = GetComponentInChildren<Light2D>();
        AudioController = GetComponentInChildren<AudioPlayerController>();
        GlobalLight.intensity = globalLightMin;
    }



    private IEnumerator TurnLightsOn()
    {
        while (globalLightMax > GlobalLight.intensity)
        {
            GlobalLight.intensity += lightIncrementsPerTick;
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
        yield break;
    }
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
    }

    private void EnterDetectedPhase()
    {
        Detected = true;
        AudioController.PlayDetectedMusic();
        StartCoroutine(TurnLightsOn());
        PublishEnemyStateChange(EnemyStates.Detected);
        PlayerWasDetected?.Invoke();
    }

}

public interface ILevelManager
{
    void PlayerIsBeingSeen(float distance);
    void SoundWasHeard();
    void PublishEnemyStateChange(EnemyStates state);
}
