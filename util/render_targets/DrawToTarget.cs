using Godot;

/// <summary>
/// Script that causes a sprite to be moved to a viewport registered as a render target.
/// See RenderTargets.
/// </summary>
public class DrawToTarget : Node2D
{
    [Export]
    private string _target = RenderTargets.GAMEPLAY_VIEWPORT;

    private bool _started = false;
    private RemoteTransform2D _tranform;

    public override void _Ready()
    {
        base._Ready();
        
        CallDeferred(nameof(MoveToTarget));
    }

    private void MoveToTarget()
    {
        RenderTargets.Get(_target).MatchSome(viewport =>
        {
            var startParent = GetParent();
            
             _tranform = new RemoteTransform2D();
            startParent.AddChild(_tranform);
            _tranform.Position = Position;
            _tranform.Rotation = Rotation;
            _tranform.Scale = Scale;
            _tranform.RemotePath = GetPath();
            
            startParent.RemoveChild(this);
            viewport.AddChild(this);
        });
        _started = true;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        
        if (_started && !IsInstanceValid(_tranform))
            QueueFree();
    }
}
