using System;
using System.Reflection;
using Godot;

/// <summary>
/// Attribute for floating point live values in a range.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class LiveValueRange : Attribute, ILiveValue
{
    public string VariableName{ get; set; } = "Var";
    
    public string Category { get; set; }

    public float Min { get; set; }
    
    public float Max { get; set; }
    
    public float Value { get; set; }
    
    public FieldInfo Field = null;

    /// <summary>
    /// Creates a range live value from a minimum, maximum, and category.
    /// </summary>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <param name="category">The category of value</param>
    public LiveValueRange(float min, float max, string category)
    {
        Min = min;
        Max = max;
        Value = min;
        Category = category;
    }
    
    public Variant ToVariant()
    {
        return Variant.From(Value);
    }

    public void SetFromVariant(Variant variant)
    {
        Value = variant.AsSingle();
        Field.SetValue(null, Value);
    }

    public Control CreateEditorItem(LiveValuesModel model)
    {
        var editor = GD.Load<PackedScene>("res://util/live_values/range_item.tscn").Instantiate<LVRangeItem>();
        editor.Connect(model, this);
        return editor;
    }
}
