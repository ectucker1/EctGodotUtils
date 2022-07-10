using System;
using System.Threading.Tasks;
using Godot;

/// <summary>
/// A asynchronously running code block.
/// Can be used for animations, attacks, or pretty much anything that needs to be spread across frames.
/// AsyncRoutines are automatically be stopped when their owner nodes are removed from the scene tree.
/// They can also be cancelled midway through - function code can handle TaskCanceledException for this case.
/// Call AsyncRoutine.Start to begin.
/// </summary>
public class AsyncRoutine : Reference
{
    private readonly Node _owner;
    /// <summary>
    /// The Node which owns this async routine.
    /// </summary>
    public Node Owner => _owner;

    private bool _canceled = false;
    /// <summary>
    /// Whether or not this routine has been canceled.
    /// </summary>
    public bool Canceled => _canceled;
    
    private AsyncRoutine(Node owner)
    {
        _owner = owner;
    }

    /// <summary>
    /// Creates a new AsyncRoutine owned by the given node,
    /// and starts the given asynchronous task.
    /// </summary>
    /// <param name="owner">The owner node of the routine</param>
    /// <param name="task">A function to run the task given the created routine</param>
    public static AsyncRoutine Start(Node owner, Func<AsyncRoutine, Task> task)
    {
        AsyncRoutine routine = new AsyncRoutine(owner);
        routine.Run(task);
        return routine;
    }

    /// <summary>
    /// Cancels this routine, causing it to stop next time an await is hit.
    /// This can be caught as a TaskCanceledException in the code for the task.
    /// </summary>
    public void Cancel()
    {
        _canceled = true;
    }
    
    /// <summary>
    /// Waits for a period of time in seconds.
    /// </summary>
    /// <param name="time">The amount of time to wait in seconds.</param>
    /// <param name="pauseModeProcess">Whether or not to count down the time while the game is paused</param>
    public async Task Delay(float time, bool pauseModeProcess = false)
    {
        await ToSignal(_owner.GetTree().CreateTimer(time, pauseModeProcess), "timeout");
        ThrowIfStopped();
    }

    /// <summary>
    /// Waits for the next idle frame for the engine.
    /// This is equivalent to the next _process.
    /// </summary>
    /// <returns>The time since the last _process, in seconds.</returns>
    public async Task<float> IdleFrame()
    {
        do
        {
            await ToSignal(_owner.GetTree(), "idle_frame");
            ThrowIfStopped();
        } while (_owner.GetTree().Paused);
        return _owner.GetProcessDeltaTime();
    }
    
    /// <summary>
    /// Waits for the next physics frame for the engine.
    /// This is equivalent to the next _physics_process.
    /// </summary>
    /// <returns>The time since the last _physics_process, in seconds.</returns>
    public async Task<float> PhysicsFrame()
    {
        do
        {
            await ToSignal(_owner.GetTree(), "physics_frame");
            ThrowIfStopped();
        } while (_owner.GetTree().Paused);
        return _owner.GetPhysicsProcessDeltaTime();
    }

    /// <summary>
    /// Waits for the given condition to become true, checking every physics_process.
    /// </summary>
    /// <param name="predicate">A predicate function representing the condition</param>
    public async Task Condition(Func<bool> predicate)
    {
        while (!predicate())
            await PhysicsFrame();
    }

    /// <summary>
    /// Waits for the given signal from the given source.
    /// </summary>
    /// <param name="source">The source of the signal</param>
    /// <param name="signal">The signal to wait for</param>
    public async Task Signal(Godot.Object source, string signal)
    {
        await ToSignal(source, signal);
        ThrowIfStopped();
    }
    
    private void ThrowIfStopped()
    {
        if (!IsInstanceValid(_owner) || !_owner.IsInsideTree())
            throw new TaskOwnerInvalidException();
        if (_canceled)
            throw new TaskCanceledException();
    }
    
    /// <summary>
    /// An exception meaning that the owner node of a task has been destroyed
    /// </summary>
    public class TaskOwnerInvalidException : OperationCanceledException { }
    
    private async Task Run(Func<AsyncRoutine, Task> task)
    {
        try
        {
            await task(this);
        }
        catch (TaskCanceledException) { }
        catch (TaskOwnerInvalidException) { }
    }
}
