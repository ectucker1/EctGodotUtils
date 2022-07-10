using Godot;

/// <summary>
/// The client node that applies live values from JSON.
/// </summary>
public class LiveValuesClient : Node
{
    private LiveValuesModel _model;

    private float _lastLoad = Mathf.Inf;
    
    public override void _Ready()
    {
        base._Ready();
        _model = new LiveValuesModel();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        
        _lastLoad += delta;
        if (_lastLoad > 1.0f)
        {
            _model.LoadJSON(false);
            _lastLoad = 0;
        }
    }
}
