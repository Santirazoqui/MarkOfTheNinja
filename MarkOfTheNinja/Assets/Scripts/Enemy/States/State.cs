using Assets.Scripts.Enemy.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected StateContext _lastRecivedContext;
    public void Enter(StateContext context)
    {
        _lastRecivedContext = context;
        _Enter();
    }
    protected abstract void _Enter();
        
    /// <summary>
    /// Alias para el Update()
    /// </summary>    
    public void Do(StateContext context)
    {
        _lastRecivedContext = context;
        _Do();
    }

    protected abstract void _Do();
    /// <summary>
    /// Alias para el FixedDo()
    /// </summary>
    public void FixedDo(StateContext context)
    {
        _lastRecivedContext = context;
        _FixedDo();
    }

    protected abstract void _FixedDo();

    /// <summary>
    /// Para hacer algo antes de tracisionar de estado. Bastante explicito el nombre
    /// </summary>
    public void Exit(StateContext context)
    {
        _lastRecivedContext = context;
        _Exit();
    }

    protected abstract void _Exit();
    public abstract void TriggerEnter(Collider2D collision);

}
