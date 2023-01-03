using Godot;

public class StateTestJumpState : StateNode<StateTestPlayer>
{
    public StateTestJumpState(StateTestPlayer player) : base(player)
    {

    }

    public override void Enter()
    {
        StateOwner.Modulate = Colors.Red;
        StateOwner.Velocity.y = -200;
    }

    public override void Exit()
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (StateOwner.Velocity.y > 0)
        {
            StateOwner.MovementState.ReplaceState(new StateTestFallState(StateOwner));
        }
    }
}