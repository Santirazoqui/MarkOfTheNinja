using Assets.Scripts.Enemy;
using Assets.Scripts.Enemy.Pathfinding;
using Assets.Scripts.Enemy.States;
using AYellowpaper.SerializedCollections;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
public enum EnemyStates
{
    Chilling, GoingAtSound, DetectedPatrolling, Confused, SearchingAtSound, Waiting, Stunned
}

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public EnemyStates defaultState = EnemyStates.Chilling;
    private readonly string fireBallOriginName = "fireBallOrigin";

    private State currentState;
    private EnemyStates currentStateName;
    public EnemyStates CurrentState { get => currentStateName; }
    private StateContext context;
    private Pathfinder pathfinder;
    private LevelManagerController levelManagerController;
    private IEnemyAnimationController enemyAnimationController;
    private VisionConeController visionCone;
    private GameObject fireBallOrigin;
    private DetectionRateManager detectionRateManager;

    [SerializedDictionary("Posible enemy states", "State")]
    public SerializedDictionary<EnemyStates, State> posibleStates;


    // Start is called before the first frame update
    void Start()
    {
        InitializeAnimator();
        InitializeStates();
        SetInitialState();
        SubscribeToLevelController();
    }

    private void InitializeAnimator()
    {
        IEnemyAnimationController controller = GetComponent<IEnemyAnimationController>();
        enemyAnimationController = controller;
    }


    // Update is called once per frame
    void Update()
    {
        currentState.Do(context);
    }

    private void FixedUpdate()
    {
        currentState.FixedDo(context);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.TriggerEnter(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.CollitionEnter(collision);
    }

    public void ChangeStates(EnemyStates state)
    {
        if (currentStateName == EnemyStates.DetectedPatrolling) return; // Ignores enqued pathfinding changes when in detected mode. BugFix
        SudoChangeStates(state);
    }

    public void ChangeStates(EnemyStates state, StateContext context)
    {
        this.context = context;
        ChangeStates(state);
    }

    public void PlayerIsBeingSeen(float distance)
    {
        currentState.PlayerIsBeingSeen(distance);
    }

    public void PlayerLeftVisionRadius(Vector2 lastPlayerPosition)
    {
        currentState.PlayerLeftVisionRadius(lastPlayerPosition);
    }


    //NO BORRAR - LO USAN LAS ANIMACIONES 
    public void AnimationEventFired(string eventDescription)
    {
        currentState.AnimationEventFired(eventDescription);
    }

    private void InitializeStates()
    {
        detectionRateManager = GetComponent<DetectionRateManager>();
        pathfinder = gameObject.AddComponent<Pathfinder>();
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        visionCone = GetComponentInChildren<VisionConeController>();
        Transform eyesTransform = transform.Find(fireBallOriginName);
        var instaHitboxes = GetComponentsInChildren<InstaDetectionHitboxController>();
        fireBallOrigin = eyesTransform.gameObject;
        context = new(this,
                    pathfinder,
                    player,
                    levelManagerController, 
                    enemyAnimationController,
                    visionCone,
                    fireBallOrigin,
                    detectionRateManager,
                    instaHitboxes);
        foreach (var (_, state) in posibleStates) state.SetActive(false); 
    }

    private void SetInitialState()
    {
        currentState = posibleStates[defaultState];
        currentStateName = defaultState;
        currentState.SetActive(true);
        currentState.Enter(context);
        pathfinder.TakeSnapshot();
    }

    private void SubscribeToLevelController()
    {
        var controller = FindAnyObjectByType<LevelManagerController>();
        controller.StateChanged += SudoChangeStates;
        controller.LevelWasReset += OnReset;
    }

    private void OnReset()
    {
        visionCone.ActivateDetectionCone();
        SudoChangeStates(EnemyStates.Chilling);
        pathfinder.ResetToSnapshot();
    }

    
    private void SudoChangeStates(EnemyStates state)
    {
        currentState.Exit(context);
        currentState.SetActive(false);
        currentState = posibleStates[state];
        currentStateName = state;
        currentState.SetActive(true);
        currentState.Enter(context);
    }

}


