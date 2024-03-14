using Godot;

public record MidiControlChannel
{
    public readonly int Channel;
    public readonly int Controller;
    public readonly float Min;
    public readonly float Max;

    public MidiControlChannel(int channel, int controller, float min, float max)
    {
        Channel = channel;
        Controller = controller;
        Min = min;
        Max = max;
    }
    
    public MidiControlChannel(MidiControlChannel prev, float min, float max)
    {
        Channel =  prev.Channel;
        Controller =  prev.Controller;
        Min = min;
        Max = max;
    }
        
    public bool Matches(InputEventMidi midiEvent)
    {
        return midiEvent.Message == MidiMessage.ControlChange
               && midiEvent.Channel == Channel
               && midiEvent.ControllerNumber == Controller;
    }

    public float ScaleValue(InputEventMidi midiEvent)
    {
        return Min + (Max - Min) * (midiEvent.ControllerValue / 127.0f);
    }

    public override string ToString()
    {
        return $"CHNL {Channel} CC {Controller}";
    }
}
