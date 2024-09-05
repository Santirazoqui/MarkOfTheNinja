using Assets.Scripts.Enemy.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    
    /// <summary>
    ///  Alias para el Start() 
    /// </summary>
    public abstract void Enter(StateContext context);
    /// <summary>
    /// Alias para el Update()
    /// </summary>    
    public abstract void Do(StateContext context);
    /// <summary>
    /// Alias para el FixedDo()
    /// </summary>
    public abstract void FixedDo(StateContext context);
    /// <summary>
    /// Para hacer algo antes de tracisionar de estado. Bastante explicito el nombre
    /// </summary>
    public abstract void Exit(StateContext context);
}
