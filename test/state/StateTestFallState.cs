using Godot;

public partial class StateTestFallState : StateNode<StateTestPlayer>
{
    public StateTestFallState(StateTestPlayer player) : base(player)
    {

    }

    public override void Enter()
    {
        StateOwner.Modulate = Colors.Orange;
    }

    public override void Exit()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
     
        if (StateOwner.IsOnFloor())
        {
            StateOwner.MovementState.PopState();
        }
    }
}