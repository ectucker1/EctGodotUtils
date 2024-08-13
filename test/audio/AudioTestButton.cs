using Godot;
using System;

public partial class AudioTestButton : Button
{
	private IAudioCollection _collection;
	
	public override void _Ready()
	{
		base._Ready();

		_collection = GetChild<IAudioCollection>(0);
	}

	public override void _Pressed()
	{
		_collection.Play();
	}
}
