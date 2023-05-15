using Godot;
using System;
using System.Threading.Tasks;

public partial class AsyncRoutineTest : Sprite2D
{
    private AnimationPlayer _animPlayer;

    private AsyncRoutine _routine;
    
    public override void _Ready()
    {
        base._Ready();

        _animPlayer = this.FindChild<AnimationPlayer>();
        
        _routine = AsyncRoutine.Start(this, TestRoutine);

        Button stop = GetParent().GetNode<Button>("Stop");
        stop.Connect(SignalNames.BUTTON_PRESSED, new Callable(this, nameof(_StopPressed)));
    }

    private async Task TestRoutine(AsyncRoutine routine)
    {
        DebugOverlay.AddMessage(this, "Routine", "Started");
        
        await routine.Delay(2.0f);
        
        for (int i = 0; i < 100; i++)
        {
            double delta = await routine.PhysicsFrame();
            GlobalPosition += Vector2.Right * (float) delta * 50.0f;
        }

        _animPlayer.Play("Test");
        await routine.Signal(_animPlayer, SignalNames.ANIMATION_FINISHED);
        
        for (int i = 0; i < 100; i++)
        {
            double delta = await routine.PhysicsFrame();
            GlobalPosition += Vector2.Left * (float) delta * 50.0f;
        }
        
        DebugOverlay.AddMessage(this, "Routine", "Finished");
    }

    private void _StopPressed()
    {
        _routine.Cancel();
    }
}
