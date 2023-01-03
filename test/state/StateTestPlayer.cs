using Godot;
using System;

public class StateTestPlayer : KinematicBody2D
{
	public StateStack<StateTestPlayer> MovementState;

	public Vector2 Velocity = Vector2.Zero;
	
	public override void _Ready()
	{
		base._Ready();
		
		MovementState = new StateStack<StateTestPlayer>(this, new StateTestGroundState(this));
		AddChild(MovementState);
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		// Gravity
		Velocity += Vector2.Down * 9.8f;
		
		// Move along velocity
		Velocity = MoveAndSlide(Velocity, Vector2.Up);
	}
}
