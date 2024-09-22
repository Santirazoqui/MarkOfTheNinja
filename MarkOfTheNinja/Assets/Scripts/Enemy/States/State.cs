using Assets.Scripts.Enemy.States;
using System;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected StateContext _lastRecivedContext;
    public void Enter(StateContext context)
    {
        _lastRecivedContext = context;
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

    public virtual void PlayerIsBeingSeen() { }

    public virtual void PlayerLeftVisionRadius(Vector2 lastPlayerPosition) { }
    public virtual void AnimationEventFired(string eventDescription) { }

    public void SetActive(bool value) => gameObject.SetActive(value);

    
}
