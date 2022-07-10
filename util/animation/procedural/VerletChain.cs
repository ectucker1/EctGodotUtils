using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Node creating a chain of its children simulated by verlet physics.
/// This can be used for hair and other dangling animations.
/// </summary>
public class VerletChain : Node2D
{
    private List<VerletPoint> _points;
    private List<VerletStick> _sticks;

    [Export]
    private float _gravity = 40.0f;

    [Export]
    private float _dampening = 0.995f;

    private Vector2 _lastPosition;
    
    public override void _Ready()
    {
        _points = new List<VerletPoint>();
        for (int i = 0; i < GetChildCount(); i++)
        {
            Node2D child = GetChild<Node2D>(i);
            _points.Add(new VerletPoint(child.GlobalPosition, child, i == 0));
        }

        _sticks = new List<VerletStick>();
        for (int i = 0; i < _points.Count - 1; i++)
        {
            _sticks.Add(new VerletStick(_points[i], _points[i + 1]));
        }

        _lastPosition = Position;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        Vector2 offset = _lastPosition - Position;
        foreach (VerletPoint point in _points)
        {
            if (point.Locked)
            {
                point.Position = _points[0].Target.GlobalPosition;
                point.OldPosition = _points[0].Target.GlobalPosition;
            }
            else
            {
                point.Position -= offset;
                point.OldPosition -= offset;
            }
        }
        
        ApplyForces(delta);
        ApplyConstraints(delta);

        foreach (VerletPoint point in _points)
        {
            point.Target.GlobalPosition = point.Position;
        }

        _lastPosition = Position;
    }

    private void ApplyForces(float delta)
    {
        // Forces
        foreach (VerletPoint point in _points)
        {
            if (!point.Locked)
            {
                Vector2 velocity = (point.Position - point.OldPosition) * _dampening;
                point.OldPosition = point.Position;
                point.Position += velocity;
                point.Position += Vector2.Down * _gravity * delta * delta;
            }
        }
    }

    private void ApplyConstraints(float delta)
    {
        for (int i = 0; i < 50; i++)
        {
            foreach (VerletStick stick in _sticks)
            {
                Vector2 stickCenter = (stick.Point1.Position + stick.Point2.Position) * 0.5f;
                Vector2 stickDir = (stick.Point1.Position - stick.Point2.Position).Normalized();
                if (!stick.Point1.Locked)
                    stick.Point1.Position = stickCenter + stickDir * stick.Length * 0.5f;
                if (!stick.Point2.Locked)
                    stick.Point2.Position = stickCenter - stickDir * stick.Length * 0.5f;
            }
        }
    }

    private class VerletPoint
    {
        public Vector2 OldPosition;
        public Vector2 Position;

        public readonly bool Locked;

        public readonly Node2D Target;

        public VerletPoint(Vector2 position, Node2D target, bool locked = false)
        {
            Position = position;
            OldPosition = position;
            Target = target;
            Locked = locked;
        }
    }

    private class VerletStick
    {
        public readonly VerletPoint Point1;
        public readonly VerletPoint Point2;

        public readonly float Length;
        
        public VerletStick(VerletPoint point1, VerletPoint point2)
        {
            Point1 = point1;
            Point2 = point2;
            Length = (point1.Position - point2.Position).Length();
        }
    }
}
