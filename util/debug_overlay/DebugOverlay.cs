using System;
using System.Collections.Generic;
using Godot;

/// <summary>
/// Utility for displaying debug information.
/// </summary>
public partial class DebugOverlay : Control
{
    private static DebugOverlay _instance;
    
    private SortedSet<DebugMessage> _messages = new SortedSet<DebugMessage>();
    private RichTextLabel _output;

    public static bool Shown => _instance.Visible;

    /// <summary>
    /// Adds a text message to the debug overlay.
    /// The message will display in the format "prefix: content".
    /// </summary>
    /// <param name="src">The object adding the message</param>
    /// <param name="prefix">The prefix of the message</param>
    /// <param name="content">The content of the message</param>
    /// <param name="priority">The message priority. Lower priorities are displayed first in the list</param>
    /// <param name="time">The amount of time to leave the message on screen, if not refreshed</param>
    public static void AddMessage(GodotObject src, string prefix, string content, int priority = Int32.MaxValue, float time = 0.1f)
    {
        if (_instance is null) return;
        var item = new DebugMessage(src, priority, prefix, content, Colors.White, time);
        if (!_instance._messages.Add(item))
        {
            _instance._messages.Remove(item);
            _instance._messages.Add(item);
        }
    }
    
    /// <summary>
    /// Adds a text message to the debug overlay.
    /// The message will display in the format "prefix: content".
    /// </summary>
    /// <param name="src">The object adding the message</param>
    /// <param name="prefix">The prefix of the message</param>
    /// <param name="content">The content of the message</param>
    /// <param name="color">The color to display the message with</param>
    /// <param name="priority">The message priority. Lower priorities are displayed first in the list</param>
    /// <param name="time">The amount of time to leave the message on screen, if not refreshed</param>
    public static void AddMessage(GodotObject src, string prefix, string content, Color color, int priority = Int32.MaxValue, float time = 0.1f)
    {
        if (_instance is null) return;
        var item = new DebugMessage(src, priority, prefix, content, color, time);
        if (!_instance._messages.Add(item))
        {
            _instance._messages.Remove(item);
            _instance._messages.Add(item);
        }
    }

    /// <summary>
    /// Draws a vector from the given point, in global space.
    /// </summary>
    /// <param name="from">The point to begin at</param>
    /// <param name="vector">The vector to draw</param>
    /// <param name="color">The color of the vector</param>
    public static void DrawVectorFrom(Vector2 from, Vector2 vector, Color color)
    {
        InWorldOverlay.Drawables.Add(new DebugVector(ConvertPoint(from), vector, color));
        InWorldOverlay.Instance.QueueRedraw();
    }

    /// <summary>
    /// Draws a vector between two points, in global space.
    /// </summary>
    /// <param name="from">The point to draw from</param>
    /// <param name="to">The point to draw to</param>
    /// <param name="color">The color of point to draw</param>
    public static void DrawVectorBetween(Vector2 from, Vector2 to, Color color)
    {
        InWorldOverlay.Drawables.Add(new DebugVector(ConvertPoint(from), to - from, color));
        InWorldOverlay.Instance.QueueRedraw();
    }

    /// <summary>
    /// Draws a circle at a point in global space.
    /// </summary>
    /// <param name="point">The point to draw at</param>
    /// <param name="color">The color of circle to draw</param>
    public static void DrawPoint(Vector2 point, Color color)
    {
        InWorldOverlay.Drawables.Add(new DebugPoint(ConvertPoint(point), color));
        InWorldOverlay.Instance.QueueRedraw();
    }
    
    private static Vector2 ConvertPoint(Vector2 point)
    {
        foreach (var viewport in RenderTargets.Get(RenderTargets.GAMEPLAY_VIEWPORT))
            return viewport.GetFinalTransform() * point;
        return point;
    }
    
    public override void _Ready()
    {
        base._Ready();

        _output = this.FindChild<RichTextLabel>();

        _instance = this;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        AddMessage(this, "FPS", Engine.GetFramesPerSecond().ToString());
        
        List<DebugMessage> toRemove = new List<DebugMessage>();
        _output.Clear();
        foreach (var message in _messages)
        {
            message.AddTo(_output);
            if (message.IsFinished(delta))
                toRemove.Add(message);
        }

        foreach (var message in toRemove)
        {
            _messages.Remove(message);
        }
    }
    
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("debug_show"))
            Visible = !Visible;
    }
}
