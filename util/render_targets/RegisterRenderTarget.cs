using Godot;

/// <summary>
/// Registers the viewport as a render target with it's name.
/// Will also keep the viewport scaled to the size of the root, or another viewport being copied.
/// </summary>
public partial class RegisterRenderTarget : SubViewport
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
            _copyTransform = GetNode<SubViewport>(_copyTransformPath);
        
        if (_copyTransform == null)
            _copyTransform = GetTree().Root.GetViewport();
        
        _copyTransform.Connect("size_changed", new Callable(this, nameof(_ScreenSizeChanged)));
        _ScreenSizeChanged();

        RenderTargets.Register(Name, this);
    }

    private void _ScreenSizeChanged()
    {
        Vector2 size = _copyTransform.GetVisibleRect().Size;
        Size = new Vector2I(Mathf.CeilToInt(size.X), Mathf.CeilToInt(size.Y));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_copyTransform != null)
        {
            GlobalCanvasTransform = _copyTransform.GlobalCanvasTransform;
            CanvasTransform = _copyTransform.CanvasTransform;
        }
    }
}