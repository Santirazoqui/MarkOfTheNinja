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

    public delegate void OnDetection();
    public event OnDetection DetectedModeStarted;

    public delegate void OnDetectionEnd();
    public event OnDetectionEnd DetectedModeEnded;
    public bool Detected { get; private set; }

    public int Score { get; private set; }
    public int ScoreAtLastCheckpoint { get; set; }
    public bool PlayerAlive { get => playerIsAlive; }
    public bool CanWin { get; set; } = false;

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
    private GameData gameData;
    private IEnumerator turnLightsOnCorutine =null;
    private IEnumerator detectedCounterCorutine =null;
    private ISceneSwitcher sceneSwitcher;
    private bool playerIsAlive = true;

    [Inject]
    public void Constructor(IDataAccessManager dataAccessManager, ISceneSwitcher sceneSwitcher)
    {
        this.sceneSwitcher = sceneSwitcher;
        this.dataAccessManager = dataAccessManager;
        var gameData = dataAccessManager.LoadData();
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

    private void TurnLightsOn()
    {
        if(turnLightsOnCorutine != null) StopCoroutine(turnLightsOnCorutine);
        turnLightsOnCorutine = SetGlobalLightState(true);
        StartCoroutine(turnLightsOnCorutine);
    }

    private void TurnLightsOff()
    {
        if (turnLightsOnCorutine != null) StopCoroutine(turnLightsOnCorutine);
        turnLightsOnCorutine = SetGlobalLightState(false);
        StartCoroutine(turnLightsOnCorutine);
    }

    private IEnumerator SetGlobalLightState(bool on)
    {
        if (on)
        {
            while (globalLightMax > GlobalLight.intensity)
            {
                GlobalLight.intensity += lightIncrementsPerTick;
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }
        }
        else
        {
            while (globalLightMin < GlobalLight.intensity)
            {
                GlobalLight.intensity -= lightIncrementsPerTick;
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }
        }

        yield break;
    }

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
        /*if (cameraPan != null && cameraPan.shouldPan) 
            yield return new WaitForSeconds(this.cameraPan.secondsToPan + this.cameraPan.secondsBeforePan + this.cameraPan.secondsInExitGate);*/

        while(cameraPan!=null && cameraPan.shouldPan && !cameraPan.finishedPan)
        {
            yield return null;
        }

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
        DetectedModeEnd();
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

    private Dictionary<string, int> levelHighScores = new Dictionary<string, int>();
    private void SaveData()
    {
        var score = CalculateScore();
        int currentLevelPreviousScore = gameData != null ? gameData.HighScores[SceneManager.GetActiveScene().name] : 0;
        int highScore = (currentLevelPreviousScore != null && score.Total < currentLevelPreviousScore) 
            ? currentLevelPreviousScore : score.Total;

        levelHighScores[SceneManager.GetActiveScene().name] = highScore;
        var data = new GameData
        {
            GameSceneIndex = SceneManager.GetActiveScene().buildIndex,
            PreviousLevelName = SceneManager.GetActiveScene().name,
            TimeSpentInLevel = this.TimeSpentInLevel,
            Score = score,
            HighScores = levelHighScores
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
        TurnLightsOn();
        StartDetectedCountDown();
        DetectedModeStarted?.Invoke();
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
        DetectedModeEnd();
        PublishEnemyStateChange(EnemyStates.Chilling);
        
    }

    private void DetectedModeEnd()
    {
        TurnLightsOff();
        Detected = false;
        AudioController.PlayNonDetectedMusic();
        DetectedModeEnded?.Invoke();
    }


}

public interface ILevelManager
{
    void PublishEnemyStateChange(EnemyStates state);
    void PlayerWasCaught();
}
