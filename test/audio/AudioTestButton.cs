using Godot;
using System;

public partial class AudioTestButton : Button
{
	private IAudioCollection _collection;
	
	public override void _Ready()
	{
		base._Ready();

		_collection = this.FindChild<IAudioCollection>();
	}

	public override void _Pressed()
	{
		_collection.Play();
	}
}
