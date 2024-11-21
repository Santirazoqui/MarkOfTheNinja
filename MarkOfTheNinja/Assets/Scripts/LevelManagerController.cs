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
    [SerializeField] GameObject PlayingCanvas;
    [SerializeField] GameObject DeathCanvas;

    public delegate void OnGlobalEnemyStateChange(EnemyStates state);
    public event OnGlobalEnemyStateChange StateChanged;

    public delegate void OnDetection();
    public event OnDetection PlayerWasDetected;

    public delegate void OnlevelReset();
    public event OnlevelReset LevelWasReset;

    public delegate void OnCheckpointReached(Vector2 position);
    public event OnCheckpointReached CheckpointReached;
    public float DetectionRate { get; private set; } = 0;
    public bool Detected { get; private set; }

    public int Score { get; private set; }
    public int ScoreAtLastCheckpoint { get; set; } = 0;

    [Header("Detection Rate")]
    public float visualDetectionRate = 100f;
    public float expoentialDistanceMultiplier= 2f;
    public float audioDetectionRate = 50f;
    [Header("Detection Rate Reduction")]
    public float secondsBeforeDetectionDecreases = 2f;
    public float minDecreasePerTick = 0.1f;
    public float maxDecreasePerTick = 0.2f;
    public float increaseInDecreasePerTick = 0.001f;
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
    private IEnumerator previousDetectionDecresionRoutine = null;
    private ISceneSwitcher sceneSwitcher;

    [Inject]
    public void Constructor(IDataAccessManager dataAccessManager, ISceneSwitcher sceneSwitcher)
    {
        this.sceneSwitcher = sceneSwitcher;
        this.dataAccessManager = dataAccessManager;
        previousScore = dataAccessManager.LoadData();
    }

    private void Start()
    {
        LevelWasReset += OnRestartLevel;
        CheckpointReached += OnCheckpoint;
        GlobalLight = GetComponentInChildren<Light2D>();
        AudioController = GetComponentInChildren<AudioPlayerController>();
        GlobalLight.intensity = globalLightMin;
        Score = initialPoints;
        DeathCanvas = Instantiate(DeathCanvas);
        DeathCanvas.SetActive(false);
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

    private void OnRestartLevel()
    {
        GlobalLight.intensity = globalLightMin;
        Score = ScoreAtLastCheckpoint;
        DetectionRate = 0;
        Detected = false;
        AudioController.PlayNonDetectedMusic();
        this.DeathCanvas.SetActive(false);
        this.PlayingCanvas.SetActive(true);
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
        var multiplier = distance != 0 ? 1 / (float)(Math.Pow(distance,expoentialDistanceMultiplier)) : 1;
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

        sceneSwitcher.ChangeScenes(goToScreen);
    }

    public void PlayerWasCaught()
    {
        this.PlayingCanvas.SetActive(false);
        this.DeathCanvas.SetActive(true);
        
    }

    public void CheckpointWasReached(Vector2 position)
    {
        CheckpointReached?.Invoke(position);
    }
    public void RestartLevel()
    {
        LevelWasReset?.Invoke();
    }

    private void OnCheckpoint(Vector2 position)
    {
        ScoreAtLastCheckpoint = Score;
    }

    private void SaveData()
    {
        var score = CalculateScore();
        var data = new GameData
        {
            GameSceneIndex = SceneManager.GetActiveScene().buildIndex,
            TimeSpentInLevel = this.TimeSpentInLevel,
            Score = score,
            HighScore = (previousScore.HighScore != null && score.Total < previousScore.HighScore) ? previousScore.HighScore : score.Total,
        };
        
        dataAccessManager.SaveData(data);
    }

    private Score CalculateScore()
    {
        var score = new Score()
        {
            RawPoints = Score,
            TimeBonus = TimeSpentInLevel <= minTimeToGetTimeBonus ? fastTimeScoreBonus : 0,
        };
        score.Total = score.RawPoints + score.TimeBonus; 
        return score;
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
        Detected = true;
        Score -= pointsLostWhenDetected;
        AudioController.PlayDetectedMusic();
        StartCoroutine(TurnLightsOn());
        StopDetectionDecreasion();
        PublishEnemyStateChange(EnemyStates.DetectedPatrolling);
        PublishPlayerDetection();
    }

    private void PublishPlayerDetection()
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
    private void StopDetectionDecreasion()
    {
        if (previousDetectionDecresionRoutine is not null) StopCoroutine(previousDetectionDecresionRoutine);
    }

    private IEnumerator StartSuspicionDecrease()
    {
        yield return new WaitForSeconds(secondsBeforeDetectionDecreases);
        float decrease = minDecreasePerTick;
        while(DetectionRate >0)
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

}

public interface ILevelManager
{
    void PlayerIsBeingSeen(float distance);
    void SoundWasHeard();
    void PublishEnemyStateChange(EnemyStates state);
    void PlayerWasInstaDetected();
    void PlayerWasCaught();
}
