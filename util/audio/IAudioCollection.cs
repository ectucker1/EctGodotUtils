using Godot;

/// <summary>
/// An interface representing a collection of audio streams.
/// </summary>
public interface IAudioCollection
{
    /// <summary>
    /// Returns true if audio is playing.
    /// </summary>
    bool Playing { get; }
    
    /// <summary>
    /// Plays the next audio stream in the collection.
    /// </summary>
    void Play(float from = 0.0f);

    /// <summary>
    /// Plays the next audio stream in the collection, but only if none are playing now.
    /// </summary>
    void PlayIfNot();

    /// <summary>
    /// Stops the currently playing audio.
    /// </summary>
    void Stop();

    /// <summary>
    /// Connects the Finished signal of this collection to the given callable.
    /// </summary>
    /// <param name="callable">The callable to connect to</param>
    void ConnectFinished(Callable callable);
}
