using Godot;
using System;

/// <summary>
/// Passes in a render target as a texture uniform for the shader on this sprite.
/// </summary>
public partial class UniformRenderTarget : Sprite2D
{
    /// <summary>
    /// The render target to pass in.
    /// </summary>
    [Export]
    private string _target = RenderTargets.VELOCITY_VIEWPORT;
    /// <summary>
    /// The uniform to set.
    /// </summary>
    [Export]
    private string _uniform = "VELOCITY_VIEWPORT";

    public override void _Ready()
    {
        RenderTargets.Get(_target).MatchSome(viewport =>
        {
            if (Material is ShaderMaterial shaderMaterial)
            {
                shaderMaterial.SetShaderParameter(_uniform, viewport.GetTexture());
            }
        });
    }
}
