using System;
using System.Collections.Generic;

/// <summary>
/// A type of state machine that remembers a history of prior states.
/// Will always be in one and only one active state.
/// </summary>
/// <typeparam name="T">The type of object this state machine applies to</typeparam>
public class StateStack<T> : StateMachine<T>
{
    /// <summary>
    /// Gets the currently active state in the stack.
    /// </summary>
    public StateNode<T> Top => _states.Peek();

    private readonly Stack<StateNode<T>> _states = new Stack<StateNode<T>>();
    
    /// <summary>
    /// Creates a new StateSet for the given object.
    /// </summary>
    /// <param name="owner">The object this state stack is for</param>
    /// <param name="init">The initial state</param>
    public StateStack(T owner, StateNode<T> init) : base(owner)
    {
        _states.Push(init);
        AddStateChild(init);
    }

    public override IEnumerable<StateNode<T>> GetActiveStates()
    {
        return new[] { Top };
    }
    
    /// <summary>
    /// Pushes a state onto the top of the stack, and makes it active.
    /// </summary>
    /// <param name="state">The state to add</param>
    public void PushState(StateNode<T> state)
    {
        RemoveStateChild(Top);
        _states.Push(state);
        AddStateChild(state);
    }

    /// <summary>
    /// Exits the top state of the stack and removes it.
    /// </summary>
    /// <returns>The state removed from the stack</returns>
    /// <exception cref="InvalidOperationException">If there is only one state in the stack</exception>
    public StateNode<T> PopState()
    {
        if (_states.Count <= 1)
        {
            throw new InvalidOperationException("EntityStateStack cannot be made empty");
        }

        StateNode<T> removed = _states.Pop();
        RemoveStateChild(removed);
        removed.QueueFree();
        AddStateChild(Top);
        
        return removed;
    }

    /// <summary>
    /// Pops states until a state of the given type is on top of the stack.
    /// </summary>
    /// <typeparam name="TTarget">The type of state to pop to</typeparam>
    public void PopStateTo<TTarget>()
    {
        while (!(Top is TTarget))
        {
            PopState();
        }
    }

    /// <summary>
    /// Replaces the top state on the stack with the given state.
    /// </summary>
    /// <param name="state">The state to leave on top</param>
    /// <returns>The state that was replaced</returns>
    public StateNode<T> ReplaceState(StateNode<T> state)
    {
        StateNode<T> removed = _states.Pop();
        RemoveStateChild(removed);
        removed.QueueFree();
        _states.Push(state);
        AddStateChild(state);
        
        return removed;
    }
}