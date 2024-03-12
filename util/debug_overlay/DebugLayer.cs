using Godot;

public partial class DebugLayer : CanvasLayer
{
	public static bool Shown = false;
	
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("debug_show"))
		{
			Visible = !Visible;
			Shown = Visible;
		}
	}
}
