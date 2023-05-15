using Godot;
using System;
using System.Threading.Tasks;

/// <summary>
/// Global overlay used to transition smoothly between scenes.
/// Will play the configured Out and In animations on the animation player.
/// </summary>
public partial class Transition : Control
{
    private static Transition _instance;

    private string _next;

    private AnimationPlayer _anim;

    private AsyncRoutine _routine;
    
    public override void _Ready()
    {
        base._Ready();

        _anim = this.FindChild<AnimationPlayer>();
        _instance = this;
    }

    /// <summary>
    /// Play an animation to transition to the given scene.
    /// </summary>
    /// <param name="scene">The path to the scene to transition to</param>
    public static void TransitionTo(string scene)
    {
        if (_instance._routine == null)
        {
            _instance._next = scene;
            _instance._routine = AsyncRoutine.Start(_instance, _instance.TransitionRoutine);
        }
    }

    public async Task TransitionRoutine(AsyncRoutine routine)
    {
        GetTree().Paused = true;
        PauseMenu.Enabled = false;
        
        _anim.Play("Out");
        await routine.ToSignal(_anim, SignalNames.ANIMATION_FINISHED);

        GetTree().ChangeSceneToFile(_next);
        
        _anim.Play("In");
        await routine.ToSignal(_anim, SignalNames.ANIMATION_FINISHED);

        GetTree().Paused = false;
        PauseMenu.Enabled = false;

        _routine = null;
    }
}
