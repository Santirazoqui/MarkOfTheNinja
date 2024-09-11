using Assets.Scripts.Enemy.Pathfinding;
using Assets.Scripts.Enemy.States;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public EnemyStates defaultState = EnemyStates.Chilling;

    private State currentState;
    private StateContext context;
    private Pathfinder pathfinder;
    public enum EnemyStates
    {
        Chilling, GoingAtSound, Detected
    }
    [SerializedDictionary("Posible enemy states", "State")]
    public SerializedDictionary<EnemyStates, State> posibleStates;


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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.CollitionEnter(collision);
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
        context = new(this,pathfinder,player);
    }

    private void SetInitialState()
    {
        currentState = posibleStates[defaultState];
        currentState.Enter(context);
    }


}
