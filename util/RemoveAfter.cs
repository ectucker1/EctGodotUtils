using Godot;

/// <summary>
/// Script that causes a node to be removed after a set amount of time.
/// The time is exported as a variable in the editor.
/// </summary>
public class RemoveAfter : Node
{
    private float _time = 0;

    /// <summary>
    /// The amount of time to wait before removing this node.
    /// </summary>
    [Export]
    public float Lifetime = 1.0f;
    
    public override void _Process(float delta)
    {
        base._Process(delta);
        _time += delta;
        if (_time >= Lifetime)
            QueueFree();
    }
}