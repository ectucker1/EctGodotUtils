using System;
using System.Collections.Generic;
using Godot;
using Object = Godot.Object;

/// <summary>
/// Abstract base for audio stream collections.
/// </summary>
public abstract class AAudioStreamCollection : Node, IAudioCollection
{
    protected readonly List<AudioStreamProxy> Streams = new List<AudioStreamProxy>();

    public bool Playing
    {
        get
        {
            foreach (var stream in Streams)
            {
                if (stream.Playing)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Emitted whenever an element in the collection finishes playing.
    /// </summary>
    [Signal]
    public delegate void Finished();

    public override void _Ready()
    {
        base._Ready();
        
        foreach (Node child in GetChildren())
        {
            AudioStreamProxy proxy = new AudioStreamProxy(child);
            if (proxy.Valid)
            {
                proxy.Connect(nameof(AudioStreamProxy.Finished), this, nameof(EmitFinished));
                Streams.Add(proxy);
            }
        }

        if (Streams.Count <= 0)
            throw new Exception("Audio stream collection must not be empty");
    }

    public abstract void Play(float from = 0.0f);

    public void PlayIfNot()
    {
        if (!Playing)
        {
            Play();
        }
    }

    public void Stop()
    {
        foreach (var stream in Streams)
        {
            stream.Stop();
        }
    }

    private void EmitFinished()
    {
        EmitSignal(nameof(Finished));
    }
    
    public void ConnectFinished(Object obj, string target)
    {
        Connect(nameof(Finished), obj, target);
    }
}
