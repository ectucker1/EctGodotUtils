using Godot;

/// <summary>
/// Script that causes a node to be removed after a set amount of time.
/// The time is exported as a variable in the editor.
/// </summary>
public partial class RemoveAfter : Node
{
    private double _time = 0;

    /// <summary>
    /// The amount of time to wait before removing this node.
    /// </summary>
    [Export]
    public double Lifetime = 1.0f;
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        _time += delta;
        if (_time >= Lifetime)
            QueueFree();
    }
}