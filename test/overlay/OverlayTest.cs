using Godot;
using System;

public class OverlayTest : Node2D
{
    private Node2D _sprite1;
    private Node2D _sprite2;

    public override void _Ready()
    {
        base._Ready();
        
        _sprite1 = FindNode("Sprite1") as Node2D;
        _sprite2 = FindNode("Sprite2") as Node2D;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
        DebugOverlay.AddMessage(this, "Sprite 1 Position", _sprite1.Position.ToString(), Colors.Aqua);
        if (GD.Randi() % 120 == 0)
        {
            DebugOverlay.AddMessage(this, "Event", "Something Happened", Colors.Fuchsia, 1, 0.5f);
        }
        
        DebugOverlay.DrawVectorBetween(_sprite1.Position, _sprite2.Position, Colors.Red);
        DebugOverlay.DrawVectorFrom(_sprite1.Position, Vector2.Up * 50.0f, Colors.Orange);
        DebugOverlay.DrawPoint(_sprite2.Position, Colors.ForestGreen);
    }
}
