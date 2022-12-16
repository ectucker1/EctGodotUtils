using System;

public class StatStep
{
    public object Source { get; private set; }
    public Func<float, float> Modifier { get; private set; }
    
    public StatStep(object source, Func<float, float> modifier)
    {
        Source = source;
        Modifier = modifier;
    }
}
