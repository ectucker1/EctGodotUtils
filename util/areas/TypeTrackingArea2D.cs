using System.Collections.Generic;
using Godot;

/// <summary>
/// An extension to Area2D that tracks other nodes of a generic C# type which enter it.
/// This shouldn't be attached to any nodes, but rather extended from with a concrete type.
/// </summary>
/// <typeparam name="T">The generic type to track</typeparam>
public class TypeTrackingArea2D<T> : Area2D
{
    /// <summary>
    /// The set of nodes of the tracked type currently overlapping this area.
    /// </summary>
    public HashSet<T> Tracked { get; private set; } = new HashSet<T>();

    /// <summary>
    /// Emitted whenever a new node of the tracked type enters the area.
    /// </summary>
    [Signal]
    public delegate void TypeEntered(Node2D typed);

    /// <summary>
    /// Emitted whenever a node of the tracked type exits the area.
    /// </summary>
    [Signal]
    public delegate void TypeExited(Node2D typed);
        
    public override void _Ready()
    {
        base._Ready();

        Connect(SignalNames.AREA2D_AREA_ENTERED,  this, nameof(_AreaEntered));
        Connect(SignalNames.AREA2D_AREA_EXITED, this, nameof(_AreaExited));
        
        Connect(SignalNames.AREA2D_BODY_ENTERED,  this, nameof(_BodyEntered));
        Connect(SignalNames.AREA2D_BODY_EXITED, this, nameof(_BodyExited));
    }

    private void _AreaEntered(Area2D area)
    {
        if (area is T typed)
        {
            Tracked.Add(typed);
            EmitSignal(nameof(TypeEntered), typed);
        }
    }
    
    private void _AreaExited(Area2D area)
    {
        if (area is T typed)
        {
            Tracked.Remove(typed);
            EmitSignal(nameof(TypeExited), typed);
        }
    }

    private void _BodyEntered(Node body)
    {
        if (body is T typed)
        {
            Tracked.Add(typed);
            EmitSignal(nameof(TypeEntered), typed);
        }
    }
    
    private void _BodyExited(Node body)
    {
        if (body is T typed)
        {
            Tracked.Remove(typed);
            EmitSignal(nameof(TypeExited), typed);
        }
    }
}
