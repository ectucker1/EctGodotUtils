using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Utility node that can create a breathing animation for a character made of multiple parts.
/// Animates other nodes set in the export variables.
/// </summary>
public class BreathingAnim : Node
{
    [Export]
    private NodePath _searchRoot = "";
    
    [Export]
    private string[] _breathingSearches = {};
    
    [Export]
    private float[] _breathingTimes = {};

    [Export]
    private float _topHoldTime = 0.3f;

    [Export]
    private float _bottomHoldTime = 0.3f;

    private List<Node2D>[] _sections;

    private float _time = 0.0f;
    private int _sectionIndex = 0;

    private enum BreathingState
    {
        UP,
        HOLD_UP,
        DOWN,
        HOLD_DOWN
    };
    private BreathingState _state;

    public override void _Ready()
    {
        Node root = GetNode(_searchRoot);
        
        _sections = new List<Node2D>[_breathingSearches.Length];
        for (int i = 0; i < _breathingSearches.Length; i++)
        {
            _sections[i] = new List<Node2D>();
            if (root.FindNode(_breathingSearches[i]) is Node2D node2D)
                _sections[i].Add(node2D);
        }
        
        if (_sections.Length != _breathingTimes.Length)
            SetProcess(false);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        _time += delta;
        switch (_state)
        {
            case BreathingState.HOLD_UP:
                if (_time > _topHoldTime)
                {
                    _time = 0;
                    _state = BreathingState.DOWN;
                }
                break;
            case BreathingState.HOLD_DOWN:
                if (_time > _bottomHoldTime)
                {
                    _time = 0;
                    _state = BreathingState.UP;
                }
                break;
            case BreathingState.UP:
                _processBreath(Vector2.Up, BreathingState.HOLD_UP);
                break;
            case BreathingState.DOWN:
                _processBreath(Vector2.Down, BreathingState.HOLD_DOWN);
                break;
        }
        
    }

    private void _processBreath(Vector2 direction, BreathingState nextState)
    {
        while (_time >= _breathingTimes[_sectionIndex])
        {
            foreach (var node in _sections[_sectionIndex])
                node.Position += direction;
            _sectionIndex++;
            if (_sectionIndex >= _sections.Length)
            {
                _sectionIndex = 0;
                _state = nextState;
                break;
            }
            _time = 0.0f;
        }
    }
}
