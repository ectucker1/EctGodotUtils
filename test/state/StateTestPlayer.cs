using Godot;
using System;

public partial class StateTestPlayer : CharacterBody2D
{
	public StateStack<StateTestPlayer> MovementState;

	public override void _Ready()
	{
		base._Ready();
		
		MovementState = new StateStack<StateTestPlayer>(this, new StateTestGroundState(this));
		AddChild(MovementState);
		
		UpDirection = Vector2.Up;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		// Gravity
		Velocity += Vector2.Down * 9.8f;
		
		// Move along velocity
		MoveAndSlide();
	}
}
