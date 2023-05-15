using Godot;

/// <summary>
/// A state subclass that also instances a scene as a child.
/// </summary>
/// <typeparam name="T">The type of object this state applies to</typeparam>
public abstract partial class SceneLoadingState<T> : StateNode<T>
{
    /// <summary>
    /// The scene to load, should be overriden by state implementations.
    /// </summary>
    protected abstract string ScenePath { get; }

    protected SceneLoadingState(T owner) : base(owner)
    {
        var scene = GD.Load<PackedScene>(ScenePath);
        AddChild(scene.Instantiate());
    }
}
