using System;
using Godot;

public class DebugMessage : IEquatable<DebugMessage>, IComparable<DebugMessage>
{
    private readonly ulong _srcID;
    private readonly string _prefix;
    private readonly string _content;
    private readonly int _priority;
    private readonly Color _color;
    private float _time;

    public DebugMessage(Godot.Object src, int priority, string prefix, string content, Color color, float time)
    {
        _srcID = src.GetInstanceId();
        _priority = priority;
        _prefix = prefix;
        _content = content;
        _color = color;
        _time = time;
    }

    public bool Equals(DebugMessage other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _srcID == other._srcID && _prefix == other._prefix && _priority == other._priority;
    }

    public override bool Equals(object obj)
    {
        if (obj is DebugMessage message)
            return Equals(message);
        return false;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 31 + _srcID.GetHashCode();
        hash = hash * 31 + _prefix.GetHashCode();
        hash = hash * 31 + _priority.GetHashCode();
        return hash;
    }

    public void AddTo(RichTextLabel label)
    {
        label.PushColor(_color);
        label.AddText($"{_prefix}: {_content}\n");
        label.Pop();
    }

    public int CompareTo(DebugMessage other)
    {
        if (other is null) return 1;
        if (ReferenceEquals(this, other)) return 0;
        var priorityComparison = _priority.CompareTo(other._priority);
        if (priorityComparison != 0) return priorityComparison;
        var srcIdComparison = _srcID.CompareTo(other._srcID);
        if (srcIdComparison != 0) return srcIdComparison;
        return string.Compare(_prefix, other._prefix, StringComparison.Ordinal);
    }

    public bool IsFinished(float delta)
    {
        _time -= delta;
        return _time <= 0;
    }
}
