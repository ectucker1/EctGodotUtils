using Godot;
using System;

public class AudioTestButton : Button
{
	private IAudioCollection _collection;
	
	public override void _Ready()
	{
		base._Ready();

		_collection = this.FindChild<IAudioCollection>();
		Connect(SignalNames.BUTTON_PRESSED, this, nameof(_Pressed));
	}

	private void _Pressed()
	{
		_collection.Play();
	}
}
