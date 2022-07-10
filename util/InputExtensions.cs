using Godot;

/// <summary>
/// Useful extension methods for input handling.
/// </summary>
public static class InputExtensions
{
    /// <summary>
    /// Get the "Global Position" of the mouse, but actually handling the transformation of the viewport.
    /// </summary>
    /// <param name="node">The node calling this.</param>
    /// <returns>The position of the mouse cursor in global space.</returns>
    public static Vector2 GetWorldMousePosition(this Node node)
    {
        var viewport = node.GetViewport();
        Vector2 viewportPos = viewport.GetMousePosition();
        return viewport.CanvasTransform.AffineInverse() * viewportPos;
    }
}