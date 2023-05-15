using System.Collections.Generic;
using Godot;

/// <summary>
/// An extension to Area that tracks other nodes of a generic C# type which enter it.
/// This shouldn't be attached to any nodes, but rather extended from with a concrete type.
/// </summary>
/// <typeparam name="T">The generic type to track</typeparam>
public partial class TypeTrackingArea3D<T> : Area3D
{
    /// <summary>
    /// The set of nodes of the tracked type currently overlapping this area.
    /// </summary>
    public HashSet<T> Tracked { get; private set; } = new HashSet<T>();

    /// <summary>
    /// Emitted whenever a new node of the tracked type enters the area.
    /// </summary>
    [Signal]
    public delegate void TypeEnteredEventHandler(Node3D typed);

    /// <summary>
    /// Emitted whenever a node of the tracked type exits the area.
    /// </summary>
    [Signal]
    public delegate void TypeExitedEventHandler(Node3D typed);
        
    public override void _Ready()
    {
        base._Ready();

        Connect(SignalNames.AREA2D_AREA_ENTERED, new Callable(this, nameof(_AreaEntered)));
        Connect(SignalNames.AREA2D_AREA_EXITED, new Callable(this, nameof(_AreaExited)));
        
        Connect(SignalNames.AREA2D_BODY_ENTERED, new Callable(this, nameof(_BodyEntered)));
        Connect(SignalNames.AREA2D_BODY_EXITED, new Callable(this, nameof(_BodyExited)));
    }

    private void _AreaEntered(Area2D area)
    {
        if (area is T typed)
        {
            Tracked.Add(typed);
            EmitSignal(SignalName.TypeEntered, area);
        }
    }
    
    private void _AreaExited(Area2D area)
    {
        if (area is T typed)
        {
            Tracked.Remove(typed);
            EmitSignal(SignalName.TypeExited, area);
        }
    }

    private void _BodyEntered(Node body)
    {
        if (body is T typed)
        {
            Tracked.Add(typed);
            EmitSignal(SignalName.TypeEntered, body);
        }
    }
    
    private void _BodyExited(Node body)
    {
        if (body is T typed)
        {
            Tracked.Remove(typed);
            EmitSignal(SignalName.TypeExited, body);
        }
    }
}
