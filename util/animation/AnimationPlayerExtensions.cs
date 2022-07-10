using System.Linq;
using Godot;

/// <summary>
/// Useful extension methods for the AnimationPlayer class.
/// </summary>
public static class AnimationPlayerExtensions
{
    /// <summary>
    /// Play the given animation if this player is not already playing one of that name.
    /// </summary>
    /// <param name="player">The animation player</param>
    /// <param name="name">The animation to start if it's not playing</param>
    public static void PlayIfNot(this AnimationPlayer player, string name)
    {
        if (player.CurrentAnimation != name)
            player.Play(name);
    }
    
    /// <summary>
    /// Queue the given animation if this player is not already playing one of that name.
    /// </summary>
    /// <param name="player">The animation player</param>
    /// <param name="name">The animation to start if it's not playing</param>
    public static void QueueIfNot(this AnimationPlayer player, string name)
    {
        if (player.CurrentAnimation != name && !player.GetQueue().Contains(name))
            player.Queue(name);
    }
}