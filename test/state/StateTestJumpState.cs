using Godot;

public partial class StateTestJumpState : StateNode<StateTestPlayer>
{
    public StateTestJumpState(StateTestPlayer player) : base(player)
    {

    }

    public override void Enter()
    {
        StateOwner.Modulate = Colors.Red;
        StateOwner.Velocity = new Vector2(StateOwner.Velocity.X, -200);
    }

    public override void Exit()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (StateOwner.Velocity.Y > 0)
        {
            StateOwner.MovementState.ReplaceState(new StateTestFallState(StateOwner));
        }
    }
}