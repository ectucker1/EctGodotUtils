using Godot;

/// <summary>
/// Registers the viewport as a render target with it's name.
/// Will also keep the viewport scaled to the size of the root, or another viewport being copied.
/// </summary>
public class RegisterRenderTarget : Viewport
{
    /// <summary>
    /// Path to a viewport to copy the camera and size of.
    /// </summary>
    [Export]
    private NodePath _copyTransformPath;
    
    private Viewport _copyTransform;
    
    public override void _Ready()
    {
        if (_copyTransformPath != null)
            _copyTransform = GetNode<Viewport>(_copyTransformPath);
        
        if (_copyTransform == null)
            _copyTransform = GetTree().Root;
        
        _copyTransform.Connect("size_changed", this, nameof(_ScreenSizeChanged));
        _ScreenSizeChanged();

        RenderTargets.Register(Name, this);
    }

    private void _ScreenSizeChanged()
    {
        Size = _copyTransform.GetVisibleRect().Size;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (_copyTransform != null)
        {
            GlobalCanvasTransform = _copyTransform.GlobalCanvasTransform;
            CanvasTransform = _copyTransform.CanvasTransform;
        }
    }
}