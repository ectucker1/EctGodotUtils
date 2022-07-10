using System.Collections.Generic;

/// <summary>
/// A type of state machine that can be in any number of states concurrently.
/// </summary>
/// <typeparam name="T">The type of object this state machine applies to</typeparam>
public class StateSet<T> : StateMachine<T>
{
    /// <summary>
    /// Creates a new StateSet for the given object.
    /// </summary>
    /// <param name="owner">The object this state set is for</param>
    public StateSet(T owner) : base(owner) { }
    
    public override IEnumerable<StateNode<T>> GetActiveStates()
    {
        foreach (var child in GetChildren())
        {
            if (child is StateNode<T> state)
                yield return state;
        }
    }

    /// <summary>
    /// Adds a state into this set and enters it.
    /// </summary>
    /// <param name="state">The state to add</param>
    public void AddState(StateNode<T> state)
    {
        AddStateChild(state);
        SortStates();
    }

    /// <summary>
    /// Exits a state and removes it from this set.
    /// </summary>
    /// <param name="state">The state to remove</param>
    public void RemoveState(StateNode<T> state)
    {
        RemoveStateChild(state);
        state.QueueFree();
    }

    private void SortStates()
    {
        List<StateNode<T>> states = new List<StateNode<T>>(GetActiveStates());
        states.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        for (int i = 0; i < states.Count; i++)
        {
            MoveChild(states[i], i);
        }
    }
}