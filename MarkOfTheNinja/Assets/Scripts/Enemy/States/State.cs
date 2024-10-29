using Assets.Scripts.Enemy.Pathfinding;
using Assets.Scripts.Enemy;
using Assets.Scripts.Enemy.States;
using System;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected float minDistance = 0.1f;
    protected StateContext _lastRecivedContext;
    protected IPathfinder pathfinder;
    protected EnemyController parent;
    protected GameObject player;
    protected ILevelManager levelManagerController;
    protected EnemyAnimationController animationController;
    public void Enter(StateContext context)
    {
        _lastRecivedContext = context;
        OverideVariabeles();
        EnterImplementation();
    }
    protected virtual void EnterImplementation() { }

    /// <summary>
    /// Alias para el Update()
    /// </summary>    
    public void Do(StateContext context)
    {
        _lastRecivedContext = context;
        DoImplementation();
    }

    protected virtual void DoImplementation() { }
    /// <summary>
    /// Alias para el FixedDo()
    /// </summary>
    public void FixedDo(StateContext context)
    {
        _lastRecivedContext = context;
        FixedDoImplementation();
    }

    protected virtual void FixedDoImplementation() { }

    /// <summary>
    /// Para hacer algo antes de tracisionar de estado. Bastante explicito el nombre
    /// </summary>
    public void Exit(StateContext context)
    {
        _lastRecivedContext = context;
        ExitImplementation();
    }

    protected virtual void ExitImplementation() { }
    public virtual void TriggerEnter(Collider2D collision) { }

    public virtual void CollitionEnter(Collision2D collision) { }

    public virtual void PlayerIsBeingSeen(float distance) { }

    public virtual void PlayerLeftVisionRadius(Vector2 lastPlayerPosition) { }
    public virtual void AnimationEventFired(string eventDescription) { }

    public void SetActive(bool value) => gameObject.SetActive(value);

    private void OverideVariabeles()
    {
        pathfinder = _lastRecivedContext.Pathfinder;
        parent = _lastRecivedContext.Parent;
        player = _lastRecivedContext.Player;
        levelManagerController = _lastRecivedContext.LevelManagerController;
        animationController = _lastRecivedContext.AnimationController;
    }
    
}
