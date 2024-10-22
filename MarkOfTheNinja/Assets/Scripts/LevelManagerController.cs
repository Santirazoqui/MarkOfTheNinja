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
    public float minTimeToGetTimeBonus = 20;
    public int fastTimeScoreBonus = 300;
    public float EnemySuspicionPercentage {  get; private set; }
    public float TimeSpentInLevel { get; private set; } = 0;

    private Light2D GlobalLight { get; set; }
    private AudioPlayerController AudioController { get; set; }

    private IDataAccessManager dataAccessManager;
    private GameData previousScore;

    [Inject]
    public void Constructor(IDataAccessManager dataAccessManager)
    {
        this.dataAccessManager = dataAccessManager;
        previousScore = dataAccessManager.LoadData();
    }

    private void Start()
    {
        GlobalLight = GetComponentInChildren<Light2D>();
        AudioController = GetComponentInChildren<AudioPlayerController>();
        GlobalLight.intensity = globalLightMin;
        Score = initialPoints;
        StartCoroutine(StartTimer());
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

    private IEnumerator StartTimer()
    {
        while(true)
        {
            TimeSpentInLevel += Time.deltaTime;
            yield return null;
        }
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
        StopAllCoroutines();
        SaveData();
        
        SceneManager.LoadScene(goToScreen);
    }

    private void SaveData()
    {
        var score = CalculateScore();
        var data = new GameData
        {
            GameSceneIndex = SceneManager.GetActiveScene().buildIndex,
            TimeSpentInLevel = this.TimeSpentInLevel,
            Score = score,
            HighScore = (previousScore.HighScore != null && score < previousScore.HighScore) ? previousScore.HighScore : score,
        };
        
        Debug.Log("Data being sent to be saved:");
        Debug.Log($"GameSceneIndex: {data.GameSceneIndex}, TimeSpentInLevel: {data.TimeSpentInLevel}, Score: {data.Score}, HighScore: {data.HighScore}");
        dataAccessManager.SaveData(data);
    }

    private int CalculateScore()
    {
        var score = Score;
        if(TimeSpentInLevel <= minTimeToGetTimeBonus)
        {
            score += fastTimeScoreBonus;
        }
        return score;
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
