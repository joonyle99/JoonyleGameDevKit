using System;
using UnityEngine;
using JoonyleGameDevKit;

public class Transition<T> where T : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private StateBase<T> _from;
    public StateBase<T> From => _from;

    /// <summary>
    /// 
    /// </summary>
    private StateBase<T> _to;
    public StateBase<T> To => _to;

    /// <summary>
    /// 
    /// </summary>
    private Func<bool> _condition;
    public Func<bool> Condition => _condition;

    public Transition(StateBase<T> from, StateBase<T> to, Func<bool> condition)
    {
        _from = from;
        _to = to;
        _condition = condition;
    }
}
