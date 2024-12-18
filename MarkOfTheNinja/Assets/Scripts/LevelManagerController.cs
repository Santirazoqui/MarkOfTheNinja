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

    [SerializeField] CameraPan cameraPan;

    public delegate void OnGlobalEnemyStateChange(EnemyStates state);
    public event OnGlobalEnemyStateChange StateChanged;

    public delegate void OnlevelReset();
    public event OnlevelReset LevelWasReset;

    public delegate void OnCheckpointReached(Vector2 position);
    public event OnCheckpointReached CheckpointReached;
    public bool Detected { get; private set; }

    public int Score { get; private set; }
    public int ScoreAtLastCheckpoint { get; set; }

    public bool canWin { get; set; } = false;

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
    [Header("Detected mode")]
    public float detectedModeDurationInSeconds;
    public bool reDetectionRestartsCounter = true;
    public float EnemySuspicionPercentage {  get; private set; }
    public float TimeSpentInLevel { get; private set; } = 0;
    private Light2D GlobalLight { get; set; }
    private AudioPlayerController AudioController { get; set; }

    private IDataAccessManager dataAccessManager;
    private GameData previousScore;
    private IEnumerator turnLightsOnCorutine =null;
    private IEnumerator detectedCounterCorutine =null;
    private ISceneSwitcher sceneSwitcher;
    private bool playerIsAlive = true;

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
        ScoreAtLastCheckpoint = Score;
        DeathCanvas = Instantiate(DeathCanvas);
        DeathCanvas.SetActive(false);
        StartCoroutine(StartTimer());
        //StartDebug();
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

    private int prevScore;
    private float prevDetection;
    private bool previousDetected;
    private void StartDebug()
    {
        prevScore = Score;
        previousDetected = Detected;
    }

    /*private void Update()
    {
        if (prevScore!= Score) Debug.Log("Score changed: " + Score);
        if(prevDetection !=DetectionRate) Debug.Log("Detection rate changed: "+ DetectionRate);
        if (previousDetected != Detected) Debug.Log("Detected changed: " + Detected);
        prevDetection = DetectionRate;
        prevScore = Score;
        previousDetected = Detected;
    }
    */
    private void OnRestartLevel()
    {
        GlobalLight.intensity = globalLightMin;
        Score = ScoreAtLastCheckpoint;
        if (turnLightsOnCorutine != null) StopCoroutine(turnLightsOnCorutine);
        if( detectedCounterCorutine !=null) StopCoroutine(detectedCounterCorutine);
        Detected = false;
        AudioController.PlayNonDetectedMusic();
        playerIsAlive = true;
        this.DeathCanvas.SetActive(false);
        this.PlayingCanvas.SetActive(true);
    }

    private IEnumerator StartTimer()
    {
        if (cameraPan != null && cameraPan.shouldPan) 
            yield return new WaitForSeconds(this.cameraPan.secondsToPan + this.cameraPan.secondsBeforePan); //+1 porque hay un segundo de espera antes de empezar a panear

        while(true)
        {
            if(playerIsAlive) TimeSpentInLevel += Time.deltaTime;
            yield return null;
        }
    }
    public void PublishEnemyStateChange(EnemyStates state)
    {
        StateChanged?.Invoke(state);
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
        AudioController.PlayGameOverMusic();
        playerIsAlive = false;
        
    }

    public void CheckpointWasReached(Vector2 position)
    {
        CheckpointReached?.Invoke(position);
    }
    public void RestartLevel()
    {
        LevelWasReset.Invoke();
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

    public void EnterDetectedPhase()
    {
        //Debug.Log("Enter detected phase");
        Detected = true;
        Score -= pointsLostWhenDetected;
        AudioController.PlayDetectedMusic();
        turnLightsOnCorutine = TurnLightsOn();
        StartCoroutine(turnLightsOnCorutine);
        StartDetectedCountDown();
        //StopDetectionDecreasion();
        //PublishEnemyStateChange(EnemyStates.DetectedPatrolling);
    }

    private void StartDetectedCountDown()
    {
        if(reDetectionRestartsCounter)
        {
            if(detectedCounterCorutine!=null) StopCoroutine(detectedCounterCorutine);
            detectedCounterCorutine = DetectionCountDownCorutine();
            StartCoroutine(detectedCounterCorutine);
        }
        else if(detectedCounterCorutine==null) 
        {
            detectedCounterCorutine = DetectionCountDownCorutine();
            StartCoroutine(detectedCounterCorutine);
        }
    }

    private IEnumerator DetectionCountDownCorutine()
    {
        yield return new WaitForSeconds(detectedModeDurationInSeconds);
        GlobalLight.intensity = globalLightMin;
        Detected = false;
        AudioController.PlayNonDetectedMusic();
        PublishEnemyStateChange(EnemyStates.Chilling);
    }


}

public interface ILevelManager
{
    void PublishEnemyStateChange(EnemyStates state);
    void PlayerWasCaught();
}
