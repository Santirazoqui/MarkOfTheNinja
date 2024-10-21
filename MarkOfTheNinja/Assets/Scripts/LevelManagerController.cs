using Assets.Scripts.DataAccess;
using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Zenject;


public class LevelManagerController : SubscribeOnUpdate, ILevelManager
{
    public delegate void OnGlobalEnemyStateChange(EnemyStates state);
    public event OnGlobalEnemyStateChange StateChanged;

    public delegate void OnDetection();
    public event OnDetection PlayerWasDetected;
    public float DetectionRate { get; private set; } = 0;
    public bool Detected { get; private set; }

    public int Score { get; private set; }

    [Header("Detection Rate")]
    public float visualDetectionRate = 100f;
    public float audioDetectionRate = 50f;
    [Header("Global light")]
    public float globalLightMin = 0.1f;
    public float globalLightMax = 0.5f;
    private readonly float lightIncrementsPerTick = 0.01f;
    [Header("Points")]
    public int initialPoints = 1000;
    public int pointsPerCoin = 100;
    public int pointsLostWhenDetected = 500;
    
    public float EnemySuspicionPercentage {  get; private set; }

    private Light2D GlobalLight { get; set; }
    private AudioPlayerController AudioController { get; set; }

    private IDataAccessManager dataAccessManager;

    [Inject]
    public void Constructor(IDataAccessManager dataAccessManager)
    {
        this.dataAccessManager = dataAccessManager;
    }

    private void Start()
    {
        GlobalLight = GetComponentInChildren<Light2D>();
        AudioController = GetComponentInChildren<AudioPlayerController>();
        GlobalLight.intensity = globalLightMin;
        Score = initialPoints;
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

    public void PickedUpCoin()
    {
        Score += pointsPerCoin;
    }

    public void PlayerEndedLevel(string goToScreen)
    {
        dataAccessManager.SaveData(new GameData() { Score = this.Score, GameSceneIndex = SceneManager.GetActiveScene().buildIndex });
        SceneManager.LoadScene(goToScreen);
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
        Score -= pointsLostWhenDetected;
        AudioController.PlayDetectedMusic();
        StartCoroutine(TurnLightsOn());
        PublishEnemyStateChange(EnemyStates.Detected);
        PublishPlayerDetection();
    }

    private void PublishPlayerDetection()
    {
        PlayerWasDetected?.Invoke();
    }

}

public interface ILevelManager
{
    void PlayerIsBeingSeen(float distance);
    void SoundWasHeard();
    void PublishEnemyStateChange(EnemyStates state);
    void PlayerWasInstaDetected();
}
