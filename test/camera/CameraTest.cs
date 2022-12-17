using Godot;
using System;

public class CameraTest : Node2D
{
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (GD.Randi() % 30 == 0)
        {
            CameraEffects.Trauma += 0.2f;
        }

        if (GD.Randi() % 60 == 0)
        {
            CameraEffects.Trauma += 0.5f;
        }

        if (GD.Randi() % 20 == 0)
        {
            CameraEffects.Hitstop(0.2f);
        }

        if (GD.Randi() % 30 == 0)
        {
            CameraEffects.Kickback(Vector2.Right, 64.0f);
        }
    }
}
