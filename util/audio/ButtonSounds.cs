using Godot;
using System;

/// <summary>
/// A script for a node that plays sounds when a button is hovered on or pressed.
/// Sounds may be any kind of audio stream, collection, etc.
/// </summary>
public partial class ButtonSounds : Node
{
	private AudioStreamProxy _hoverSound;
	private AudioStreamProxy _pressedSound;
	
	public override void _Ready()
	{
		base._Ready();

		_hoverSound = new AudioStreamProxy(GetNode("Hover"));
		_pressedSound = new AudioStreamProxy(GetNode("Press"));

		if (GetParent() is BaseButton button)
		{
			button.Connect(SignalNames.CONTROL_MOUSE_ENTERED, new Callable(this, nameof(_Hover)));
			button.Connect(SignalNames.BUTTON_PRESSED, new Callable(this, nameof(_Pressed)));
		}
	}

	private void _Hover()
	{
		_hoverSound.Play();
	}

	private void _Pressed()
	{
		_pressedSound.Play();
	}
}
