using Godot;
using System;

public class TestTypeTrackingArea : TypeTrackingArea2D<TestType>
{
    public override void _Ready()
    {
        base._Ready();

        Connect(nameof(TypeEntered), this, nameof(_TypeEntered));
        Connect(nameof(TypeExited), this, nameof(_TypeExited));
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
        DebugOverlay.AddMessage(this, "Tracking", $"Types Overlaying {Tracked.Count}");
    }

    private void _TypeEntered(TestType type)
    {
        DebugOverlay.AddMessage(this, "Tracking", $"{type.Name} Entered", Colors.Green, 1, 0.5f);
    }

    private void _TypeExited(TestType type)
    {
        DebugOverlay.AddMessage(this, "Tracking", $"{type.Name} Exited", Colors.Red, 1, 0.5f);
    }
}
