using Godot;

/// <summary>
/// A stream collection that plays it's first member with an increasing pitch.
/// </summary>
public class AudioStreamPitchSequence : AAudioStreamCollection
{
    /// <summary>
    /// The time to wait between plays before resetting the sequence the base pitch.
    /// </summary>
    [Export]
    public float ResetCooldown = 1.0f;

    /// <summary>
    /// The first pitch in the sequence.
    /// </summary>
    [Export]
    public float PitchScaleBase = 1.0f;

    /// <summary>
    /// The difference in pitches between each member of the sequence.
    /// </summary>
    [Export]
    public float PitchScaleDelta = 0.1f;
    
    private float _timeSincePlayed = 0.0f;
    
    private int _nextPlay = 0;
    
    public override void Play(float from = 0)
    {
        _timeSincePlayed = 0.0f;

        AudioStreamProxy picked = Streams[0];
        picked.PitchScale = PitchScaleBase + PitchScaleDelta * _nextPlay;
        picked.Play(from);

        _nextPlay += 1;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        _timeSincePlayed += delta;
        if (_timeSincePlayed >= ResetCooldown)
        {
            _nextPlay = 0;
        }
    }
}