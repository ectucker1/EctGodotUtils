using Godot;

/// <summary>
/// Script causing a Particles2D node to begin emitting when added to the tree.
/// </summary>
public partial class EmitOnReady : GpuParticles2D
{
    public override void _Ready()
    {
        base._Ready();

        Emitting = true;
    }
}