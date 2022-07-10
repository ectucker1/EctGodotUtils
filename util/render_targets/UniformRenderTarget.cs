using Godot;
using System;

/// <summary>
/// Passes in a render target as a texture uniform for the shader on this sprite.
/// </summary>
public class UniformRenderTarget : Sprite
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
                shaderMaterial.SetShaderParam(_uniform, viewport.GetTexture());
            }
        });
    }
}
