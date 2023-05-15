using Godot;

public partial class DebugVector : IDebugDrawable
{
    public readonly Vector2 From;
    public readonly Vector2 Vector;
    public readonly Color Color;

    public DebugVector(Vector2 from, Vector2 vector, Color color)
    {
        From = from;
        Vector = vector;
        Color = color;
    }

    public void Draw(CanvasItem item)
    {
        item.DrawLine(From, From + Vector, Color);
    }
}
