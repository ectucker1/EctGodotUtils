using System;
using System.Collections.Generic;
using Godot;

/// <summary>
/// A "pipeline" of functions used to calculate some kind of statistic, e.g. Speed, Max Health, etc.
/// Each effect which changes a stat can be added to the list here.
/// </summary>
public partial class StatPipeline : GodotObject
{
    private class StatStep
    {
        public object Source { get; private set; }
        public Func<float, float> Modifier { get; private set; }
    
        public StatStep(object source, Func<float, float> modifier)
        {
            Source = source;
            Modifier = modifier;
        }
    }
    
    private List<StatStep> _steps = new List<StatStep>();

    /// <summary>
    /// Emitted whenever a this pipeline is changed.
    /// This may mean it has a new value.
    /// </summary>
    [Signal]
    public delegate void ModifiedEventHandler();
    
    /// <summary>
    /// Adds a step to the stat pipeline.
    /// </summary>
    /// <param name="source">The source of this effect on the stat</param>
    /// <param name="modifier">A function taking the old value and returning a new one</param>
    public void AddStep(object source, Func<float, float> modifier)
    {
        _steps.Add(new StatStep(source, modifier));
        
        EmitSignal(SignalName.Modified);
    }

    /// <summary>
    /// Removes all steps associated with the given source.
    /// </summary>
    /// <param name="source">The source to remove steps associated from</param>
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
        
        EmitSignal(SignalName.Modified);
    }

    /// <summary>
    /// Calculates the value of the input when transformed through each function in the pipeline.
    /// </summary>
    /// <param name="stat">The baseline value of the stat</param>
    /// <returns>The given value after passed through each function in the pipeline</returns>
    public float Apply(float stat)
    {
        foreach (var step in _steps)
        {
            stat = step.Modifier(stat);
        }
        
        return stat;
    }
}
