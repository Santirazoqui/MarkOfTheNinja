using Assets.Scripts.Enemy;
using Assets.Scripts.Enemy.Pathfinding;
using Assets.Scripts.Enemy.States;
using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
public enum EnemyStates
{
    Chilling, GoingAtSound, Detected, StayingStill, Confused
}

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public EnemyStates defaultState = EnemyStates.Chilling;

    private State currentState;
    private StateContext context;
    private Pathfinder pathfinder;
    private LevelManagerController levelManagerController;
    private EnemyAnimationController enemyAnimationController;

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
        enemyAnimationController = gameObject.AddComponent<EnemyAnimationController>();
        enemyAnimationController.animator = GetComponent<Animator>();
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
        currentState.Exit(context);
        currentState.SetActive(false);
        currentState = posibleStates[state];
        currentState.SetActive(true);
        currentState.Enter(context);
    }

    public void ChangeStates(EnemyStates state, StateContext context)
    {
        this.context = context;
        ChangeStates(state);
    }

    public void PlayerIsBeingSeen()
    {
        currentState.PlayerIsBeingSeen();
    }

    public void PlayerLeftVisionRadius(Vector2 lastPlayerPosition)
    {
        currentState.PlayerLeftVisionRadius(lastPlayerPosition);
    }

    private void InitializeStates()
    {
        pathfinder = gameObject.AddComponent<Pathfinder>();
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        context = new(this,
                    pathfinder,
                    player,
                    levelManagerController, 
                    enemyAnimationController);
        foreach (var (_, state) in posibleStates) state.SetActive(false); 
    }

    private void SetInitialState()
    {
        currentState = posibleStates[defaultState];
        currentState.SetActive(true);
        currentState.Enter(context);
    }

    private void SubscribeToLevelController()
    {
        var controller = FindAnyObjectByType<LevelManagerController>();
        controller.StateChanged += ChangeStates;
    }

}
