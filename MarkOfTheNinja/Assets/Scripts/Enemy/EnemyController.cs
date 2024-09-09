using Assets.Scripts.Enemy.Pathfinding;
using Assets.Scripts.Enemy.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private State currentState;
    private StateContext context;
    private Pathfinder pathfinder;
    public enum EnemyStates
    {
        Chilling, GoingAtSound, Detected
    }
    
    private Dictionary<EnemyStates, State> posibleStates;

    public ChillingSettings chillingSettings;
    public SearchingSettings searchingSettings;

    // Start is called before the first frame update
    void Start()
    {
        
        InitializeStates();
        SetInitialState();
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

    public void ChangeStates(EnemyStates state)
    {
        currentState.Exit(context);
        currentState = posibleStates[state];
        currentState.Enter(context);
    }

    public void ChangeStates(EnemyStates state, StateContext context)
    {
        this.context = context;
        ChangeStates(state);
    }

    private void InitializeStates()
    {
        pathfinder = gameObject.AddComponent<Pathfinder>();
        context = new(this)
        {
            Pathfinder = pathfinder
        };

        posibleStates = new Dictionary<EnemyStates, State>()
        {
            [EnemyStates.Chilling] = gameObject.AddComponent<ChillingState>(),
            [EnemyStates.GoingAtSound] = gameObject.AddComponent<SearchingState>(),
            [EnemyStates.Detected] = gameObject.AddComponent<DetectedState>(),
        };
        
    }

    private void SetInitialState()
    {
        currentState = posibleStates[EnemyStates.Chilling];
        currentState.Enter(context);
    }


}

[System.Serializable]
public class ChillingSettings
{
    public float patrollingSpeed = 200f;
    public float minDistance = 0.1f;
    public float searchingRadius = 5f;
}

[System.Serializable]
public class SearchingSettings
{
    public float searchingSpeed = 300f;
    public float minDistance = 0.1f;
}
