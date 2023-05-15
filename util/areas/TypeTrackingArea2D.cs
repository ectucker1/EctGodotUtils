using System.Collections.Generic;
using Godot;

/// <summary>
/// An extension to Area2D that tracks other nodes of a generic C# type which enter it.
/// This shouldn't be attached to any nodes, but rather extended from with a concrete type.
/// </summary>
/// <typeparam name="T">The generic type to track</typeparam>
public partial class TypeTrackingArea2D<T> : Area2D
{
    /// <summary>
    /// The set of nodes of the tracked type currently overlapping this area.
    /// </summary>
    public HashSet<T> Tracked { get; private set; } = new HashSet<T>();

    /// <summary>
    /// Emitted whenever a new node of the tracked type enters the area.
    /// </summary>
    [Signal]
    public delegate void TypeEnteredEventHandler(Node2D typed);

    /// <summary>
    /// Emitted whenever a node of the tracked type exits the area.
    /// </summary>
    [Signal]
    public delegate void TypeExitedEventHandler(Node2D typed);
        
    public override void _Ready()
    {
        base._Ready();

        AreaEntered += _AreaEntered;
        AreaExited += _AreaExited;

        BodyEntered += _BodyEntered;
        BodyExited += _BodyExited;
    }
    
    private void _AreaEntered(Node2D area)
    {
        if (area is T typed)
        {
            Tracked.Add(typed);
            EmitSignal(SignalName.TypeEntered, area);
        }
    }
    
    private void _AreaExited(Node2D area)
    {
        if (area is T typed)
        {
            Tracked.Remove(typed);
            EmitSignal(SignalName.TypeExited, area);
        }
    }

    private void _BodyEntered(Node2D body)
    {
        if (body is T typed)
        {
            Tracked.Add(typed);
            EmitSignal(SignalName.TypeEntered, body);
        }
    }
    
    private void _BodyExited(Node2D body)
    {
        if (body is T typed)
        {
            Tracked.Remove(typed);
            EmitSignal(SignalName.TypeExited, body);
        }
    }
}
