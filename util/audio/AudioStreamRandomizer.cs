using Godot;

/// <summary>
/// A stream collection which plays a random member each time.
/// </summary>
public class AudioStreamRandomizer : AAudioStreamCollection
{
    /// <summary>
    /// The central point to randomize pitches around.
    /// </summary>
    [Export]
    public float PitchScaleCenter = 1.0f;

    /// <summary>
    /// The maximum difference of a played pitch scale from the center.
    /// </summary>
    [Export]
    public float PitchScaleRange = 0.0f;

    /// <summary>
    /// The central point to randomize volumes around.
    /// </summary>
    [Export]
    public float VolumeDbCenter = 0.0f;

    /// <summary>
    /// The maximum difference of a played volume from the center.
    /// </summary>
    [Export]
    public float VolumeDbRange = 0.0f;
    
    public override void Play(float from = 0)
    {
        Stop();
        AudioStreamProxy pick = Streams[(int) (GD.Randi() % Streams.Count)];
        pick.PitchScale = (float)GD.RandRange(PitchScaleCenter - PitchScaleRange, PitchScaleCenter + PitchScaleRange);
        pick.VolumeDb = (float)GD.RandRange(VolumeDbCenter - VolumeDbRange, VolumeDbCenter + VolumeDbRange);
        pick.Play();
    }
}