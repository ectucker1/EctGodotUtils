using Godot;

/// <summary>
/// Provides common camera effects, such as screenshake and hitstop.
/// </summary>
public partial class CameraEffects : Node
{
    /// <summary>
    /// The singleton instance of these camera effects.
    /// </summary>
    public static CameraEffects Instance { get; private set; }

    private Vector2 _screenshakeOffet = Vector2.Zero;
    private Vector2 _kickbackDir = Vector2.Zero;
    private float _kickbackLength = 0.0f;

    private Vector2 _offset = Vector2.Zero;
    
    /// <summary>
    /// The current offset calculated by these effects. Should be applied to any camera using them.
    /// </summary>
    public static Vector2 Offset => Instance._offset;

    private float _trauma = 0.0f;
    
    /// <summary>
    /// The trauma value used for screenshake. Add on for an impact effect.
    /// </summary>
    public static float Trauma
    {
        get => Instance._trauma;
        set
        {
            Instance._trauma = Mathf.Clamp(value, 0.0f, 1.0f);
        }
    }

    private double _time = 0;

    private FastNoiseLite _noise;

    private float _hitstopEndTime = 0;

    public override void _Ready()
    {
        base._Ready();

        Instance = this;

        _noise = new FastNoiseLite();
        _noise.FractalOctaves = 4;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        _time += delta;
        _trauma = Mathf.Clamp((float) (Trauma - delta), 0, 1);
        _kickbackLength = Mathf.Clamp((float) (_kickbackLength - delta * 64.0f), 0, Mathf.Inf);

        _screenshakeOffet.X = 128.0f * _trauma * _trauma * _noise.GetNoise1D((float) _time);
        _screenshakeOffet.Y = 128.0f * _trauma * _trauma * _noise.GetNoise1D((float) _time);

        _offset = _screenshakeOffet + _kickbackDir * _kickbackLength;

        if (Time.GetTicksMsec() > _hitstopEndTime)
            Engine.TimeScale = 1.0f;
    }

    /// <summary>
    /// Kickback the camera in a direction, will fade over time.
    /// </summary>
    /// <param name="dir">The direction to kick back in.</param>
    /// <param name="strength">The magnitude of the kickback.</param>
    public static void Kickback(Vector2 dir, float strength)
    {
        Instance._kickbackDir = dir;
        Instance._kickbackLength = strength;
    }

    /// <summary>
    /// Freeze the game for a duration in seconds.
    /// This works by changing Engine.TimeScale.
    /// </summary>
    /// <param name="duration">The duration to freeze for</param>
    public static void Hitstop(float duration)
    {
        Engine.TimeScale = 0.0f;
        Instance._hitstopEndTime = Time.GetTicksMsec() + duration * 1000;
    }
}
