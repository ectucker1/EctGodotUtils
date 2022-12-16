using Godot;

public class TestMain : Node2D
{
    [LiveValue(LVType.RANGE, 0.0f, 1.0f, "Test")]
    public static float TEST_VALUE = 0.0f;

    public override void _Ready()
    {
        base._Ready();
        
        GD.Print(TEST_VALUE);
    }
}
