using Godot;

public class DebugPoint : IDebugDrawable
{
    public readonly Vector2 Point;
    public readonly Color Color;

    public DebugPoint(Vector2 point, Color color)
    {
        Point = point;
        Color = color;
    }

    public void Draw(CanvasItem item)
    {
        item.DrawCircle(Point, 3, Color);
    }
}
