using System;
using System.Collections.Generic;
using Godot;

public class StatPipeline : Godot.Object
{
    private List<StatStep> _steps = new List<StatStep>();

    [Signal]
    public delegate void Modified();
    
    public void AddStep(object source, Func<float, float> modifier)
    {
        _steps.Add(new StatStep(source, modifier));
        
        EmitSignal(nameof(Modified));
    }

    public void RemoveFrom(object source)
    {
        List<StatStep> toRemove = new List<StatStep>();
        foreach (var step in _steps)
        {
            if (step.Source == source)
                toRemove.Add(step);
        }
        foreach (var step in toRemove)
        {
            _steps.Remove(step);
        }
        
        EmitSignal(nameof(Modified));
    }

    public float Apply(float stat)
    {
        foreach (var step in _steps)
        {
            stat = step.Modifier(stat);
        }
        
        return stat;
    }
}
