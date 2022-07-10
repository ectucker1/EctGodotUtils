using System;
using System.Collections.Generic;
using Godot;

/// <summary>
/// An abstract state machine.
/// Could be in one or multiple states at once
/// </summary>
/// <typeparam name="T">The type of object this state machine applies to</typeparam>
public abstract class StateMachine<T> : Node2D
{
    /// <summary>
    /// The object this state machine is for.
    /// </summary>
    public T StateOwner { get; private set; }

    protected StateMachine(T owner)
    {
        StateOwner = owner;
    }

    protected void AddStateChild(StateNode<T> state)
    {
        AddChild(state);
        state.Enter();
    }

    protected void RemoveStateChild(StateNode<T> state)
    {
        RemoveChild(state);
        state.Exit();
    }

    /// <summary>
    /// Gets all the currently active states.
    /// </summary>
    /// <returns>A collection of currently active states</returns>
    public abstract IEnumerable<StateNode<T>> GetActiveStates();
    
    /// <summary>
    /// Determines whether the state machine is in a given state.
    /// </summary>
    /// <param name="type">The type of state to check for</param>
    /// <returns>True iff this machine is in the requested state</returns>
    public bool IsInState(Type type)
    {
        foreach (var state in GetActiveStates())
        {
            if (type.IsInstanceOfType(state))
                return true;
        }
        return false;
    }
}
