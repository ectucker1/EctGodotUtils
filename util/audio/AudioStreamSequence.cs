using Godot;

/// <summary>
/// A stream collection that plays its members in sequence.
/// </summary>
public partial class AudioStreamSequence : AAudioStreamCollection
{
	/// <summary>
	/// The time to wait between plays before resetting the sequence to the first element.
	/// </summary>
	[Export]
	public float ResetCooldown = 1.0f;

	/// <summary>
	/// Whether or not to repeat the sequence after playing the last element.
	/// </summary>
	[Export]
	public bool Repeat = false;
	
	private double _timeSincePlayed = 0.0f;
	
	private int _nextPlay = 0;
	
	public override void Play(float from = 0)
	{
		_timeSincePlayed = 0.0f;

		AudioStreamProxy picked = Streams[_nextPlay];
		picked.Play(from);

		if (Repeat)
		{
			_nextPlay = Mathf.Wrap(_nextPlay + 1, 0, Streams.Count);
		}
		else
		{
			_nextPlay = Mathf.Clamp(_nextPlay + 1, 0, Streams.Count - 1);
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		_timeSincePlayed += delta;
		if (_timeSincePlayed >= ResetCooldown)
		{
			_nextPlay = 0;
		}
	}
}
