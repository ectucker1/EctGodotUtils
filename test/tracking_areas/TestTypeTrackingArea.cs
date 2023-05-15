using Godot;
using System;

public partial class TestTypeTrackingArea : TypeTrackingArea2D<ITestType>
{
    public override void _Ready()
    {
        base._Ready();

        Connect(nameof(TypeEntered), new Callable(this, nameof(_TypeEntered)));
        Connect(nameof(TypeExited), new Callable(this, nameof(_TypeExited)));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        DebugOverlay.AddMessage(this, "Tracking", $"Types Overlaying {Tracked.Count}");
    }

    private void _TypeEntered(Node2D type)
    {
        DebugOverlay.AddMessage(this, "Tracking", $"{type.Name} Entered", Colors.Green, 1, 0.5f);
    }

    private void _TypeExited(Node2D type)
    {
        DebugOverlay.AddMessage(this, "Tracking", $"{type.Name} Exited", Colors.Red, 1, 0.5f);
    }
}
