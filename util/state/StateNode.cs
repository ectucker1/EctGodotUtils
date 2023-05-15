using Godot;

/// <summary>
/// A state in a state machine.
/// States are nodes and are automatically added as children of a state machine.
/// </summary>
/// <typeparam name="T">The type of object this state applies to</typeparam>
public abstract partial class StateNode<T> : Node2D
{
    /// <summary>
    /// The priority of this state in machines which support multiple at once.
    /// Higher values will be processed later.
    /// </summary>
    public int Priority = 0;
    
    /// <summary>
    /// The object this state is for.
    /// </summary>
    public T StateOwner { get; private set; }
    
    protected StateNode(T owner)
    {
        StateOwner = owner;
    }

    /// <summary>
    /// Called when the state machine enters this state.
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// Called when the state machine exits this state.
    /// </summary>
    public abstract void Exit();
}
