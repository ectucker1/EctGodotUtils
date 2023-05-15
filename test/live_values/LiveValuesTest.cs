using Godot;
using System;

public partial class LiveValuesTest : Sprite2D
{
    [LiveValue(LVType.RANGE, 0.0f, 1.0f, "Test")]
    public static float TEST_SPEED = 0.5f;

    [LiveValue(LVType.SWITCH, "Test")]
    public static bool TEST_REVERSE = false;

    public override void _Process(double delta)
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
