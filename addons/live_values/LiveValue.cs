using System;
using System.Reflection;

/// <summary>
/// Annotation applied to static variables to act as live values.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public partial class LiveValue : Attribute
{
    public LVType Type = LVType.SWITCH;
    public float Min = 0;
    public float Max = 1;
    public string Category = "Base";
    public string VariableName = "Var";
    public FieldInfo Field = null;
    public float FloatVal = 0;
    public float LastLoadFloatVal = 0;
    public bool BoolVal = false;
    public bool LastLoadBoolVal = false;

    /// <summary>
    /// Creates a live value of the given type in a given category.
    /// </summary>
    /// <param name="type">The type of value</param>
    /// <param name="category">The category of value</param>
    public LiveValue(LVType type, string category)
    {
        Type = type;
        Category = category;
    }

    /// <summary>
    /// Creates a live value of the given type with a range and category.
    /// </summary>
    /// <param name="type">The type of value</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <param name="category">The category of value</param>
    public LiveValue(LVType type, float min, float max, string category)
    {
        Type = type;
        Min = min;
        Max = max;
        Category = category;
    }
}
