using System;
using Godot;

/// <summary>
/// The client node that applies live values from JSON.
/// </summary>
public partial class LiveValuesClient : Node
{
    private LiveValuesModel _model;

    private ulong _lastLoad = UInt64.MinValue;
    
    public override void _Ready()
    {
        base._Ready();
        _model = new LiveValuesModel();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var modtime = _model.GetModtime();
        if (_lastLoad < modtime)
        {
            _model.LoadJSON(false);
            _lastLoad = modtime;
        }
    }
}
