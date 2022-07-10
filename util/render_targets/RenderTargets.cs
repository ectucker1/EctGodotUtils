using System.Collections.Generic;
using Godot;

/// <summary>
/// Provides global access to viewports registered as render targets.
/// These should ideally stay loaded between scenes.
/// </summary>
public static class RenderTargets
{
    public const string GAMEPLAY_VIEWPORT = "GameplayViewport";
    public const string VELOCITY_VIEWPORT = "VelocityViewport";

    private static Dictionary<string, Viewport> _targets = new Dictionary<string, Viewport>();

    /// <summary>
    /// Registers the given viewport as a render target with the given name.
    /// </summary>
    /// <param name="name">The name to register the target as</param>
    /// <param name="viewport">The viewport to register</param>
    public static void Register(string name, Viewport viewport)
    {
        _targets[name] = viewport;
    }
    
    /// <summary>
    /// Gets the render target with the given name, if available.
    /// </summary>
    /// <param name="target">The name of the target to get</param>
    /// <returns>An Optional of the viewport</returns>
    public static Optional<Viewport> Get(string target)
    {
        Viewport viewport;
        if (_targets.TryGetValue(target, out viewport))
            return Optional<Viewport>.Some(viewport);
        return Optional<Viewport>.None;
    }
}
