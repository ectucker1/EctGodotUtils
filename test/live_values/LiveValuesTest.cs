using Godot;
using System;

public class LiveValuesTest : Sprite
{
    [LiveValueRange(0.0f, 1.0f, "Test")]
    public static float TEST_SPEED = 0.5f;

    [LiveValueSwitch("Test")]
    public static bool TEST_REVERSE = false;

    public override void _Process(float delta)
    {
        base._Process(delta);
        
        Rotate(TEST_REVERSE ? -TEST_SPEED : TEST_SPEED);
        
        DebugOverlay.AddMessage(this, "Rotation Speed", TEST_SPEED.ToString());
        if (TEST_REVERSE)
        {
            DebugOverlay.AddMessage(this, "Reverse Rotation", "");
        }
    }
}
