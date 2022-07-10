using Godot;

/// <summary>
/// Provides common camera effects, such as screenshake and hitstop.
/// </summary>
public class CameraEffects : Node
{
    /// <summary>
    /// The singleton instance of these camera effects.
    /// </summary>
    public static CameraEffects Instance { get; private set; }

    private Vector2 _screenshakeOffet = Vector2.Zero;
    private Vector2 _kickbackDir = Vector2.Zero;
    private float _kickbackLength;

    /// <summary>
    /// The current offset calculated by these effects. Should be applied to any camera using them.
    /// </summary>
    public Vector2 Offset { get; private set; } = Vector2.Zero;

    /// <summary>
    /// The trauma value used for screenshake. Add on for an impact effect.
    /// </summary>
    public float Trauma { get; set; }

    private float _time = 0;

    private OpenSimplexNoise _noise;

    private float _hitstopEndTime = 0;

    public override void _Ready()
    {
        base._Ready();

        Instance = this;

        _noise = new OpenSimplexNoise();
        _noise.Octaves = 4;
        _noise.Period = 5.0f;
        _noise.Persistence = 0.8f;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        _time += delta;
        Trauma = Mathf.Clamp(Trauma - delta, 0, 1);
        _kickbackLength = Mathf.Clamp(_kickbackLength - delta * 64.0f, 0, Mathf.Inf);

        _screenshakeOffet.x = 128.0f * Trauma * Trauma * _noise.GetNoise1d(_time);
        _screenshakeOffet.y = 128.0f * Trauma * Trauma * _noise.GetNoise1d(_time);

        Offset = _screenshakeOffet + _kickbackDir * _kickbackLength;

        if (OS.GetTicksMsec() > _hitstopEndTime)
            Engine.TimeScale = 1.0f;
    }

    /// <summary>
    /// Kickback the camera in a direction, will fade over time.
    /// </summary>
    /// <param name="dir">The direction to kick back in.</param>
    /// <param name="strength">The magnitude of the kickback.</param>
    public void Kickback(Vector2 dir, float strength)
    {
        _kickbackDir = dir;
        _kickbackLength = strength;
    }

    /// <summary>
    /// Freeze the game for a duration in seconds.
    /// This works by changing Engine.TimeScale.
    /// </summary>
    /// <param name="duration">The duration to freeze for</param>
    public void Hitstop(float duration)
    {
        Engine.TimeScale = 0.0f;
        _hitstopEndTime = OS.GetTicksMsec() + duration * 1000;
    }
}
