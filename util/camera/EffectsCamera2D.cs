using Godot;

/// <summary>
/// Represents a camera which will have the offset from CameraEffects applied every frame.
/// Should be extended for more advanced camera behavior.
/// </summary>
public partial class EffectsCamera2D : Camera2D
{
    public override void _Process(double delta)
    { 
        base._Process(delta);
        
        Offset = CameraEffects.Offset;
    }
}
