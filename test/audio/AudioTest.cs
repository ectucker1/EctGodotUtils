using Godot;
using System;

public class AudioTest : Node2D
{
    private AudioStreamPlayer _sound;

    public override void _Ready()
    {
        base._Ready();
        
        _sound = FindNode("Sound") as AudioStreamPlayer;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (GD.Randi() % 120 == 0)
        {
            _sound.Play();
        }
    }
}
