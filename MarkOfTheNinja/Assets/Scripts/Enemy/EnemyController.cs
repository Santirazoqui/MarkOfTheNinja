using Assets.Scripts.Enemy.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private State currentState;
    private StateContext context;
    public enum EnemyStates
    {
        Chilling, GoingAtSound, Detected
    }
    private Dictionary<EnemyStates, State> posibleStates;

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

    public void ChangeStates(EnemyStates state)
    {
        currentState.Exit(context);
        currentState = posibleStates[state];
        currentState.Enter(context);
    }


    private void InitializeStates()
    {
        context = new(this);
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
