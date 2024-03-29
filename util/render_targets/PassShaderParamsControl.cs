using Godot;

/// <summary>
/// Utility for passing common extra shader parameters, such as the delta time, as uniforms.
/// </summary>
public partial class PassShaderParamsControl : Control
{
    [Export]
    private string _deltaUniform = "";

    private ShaderMaterial _material;

    public override void _Ready()
    {
        base._Ready();
        
        _material = Material as ShaderMaterial;
        if (_material == null || _deltaUniform == "")
        {
            SetProcess(false);
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_deltaUniform != "")
        {
            _material.SetShaderParameter(_deltaUniform, delta);
        }
    }
}
