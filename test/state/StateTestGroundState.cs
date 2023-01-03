using Godot;

public class StateTestGroundState : StateNode<StateTestPlayer>
{
    public StateTestGroundState(StateTestPlayer player) : base(player)
    {

    }

    public override void Enter()
    {
        StateOwner.Modulate = Colors.White;
    }

    public override void Exit()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (Input.IsActionJustPressed("ui_up"))
        {
            StateOwner.MovementState.PushState(new StateTestJumpState(StateOwner));
        }
        else if (!StateOwner.IsOnFloor())
        {
            StateOwner.MovementState.PushState(new StateTestFallState(StateOwner));
        }
    }
}