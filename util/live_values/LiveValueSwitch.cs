using System;
using System.Reflection;
using Godot;

/// <summary>
/// Attribute for switch live values.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class LiveValueSwitch : Attribute, ILiveValue
{
    public string VariableName{ get; set; } = "Var";
    
    public string Category { get; set; }
    
    public bool Value { get; set; }
    
    public FieldInfo Field = null;

    /// <summary>
    /// Creates a switch live value in the given category.
    /// </summary>
    /// <param name="category">The category of value</param>
    public LiveValueSwitch(string category)
    {
        Category = category;
    }
    
    public Variant ToVariant()
    {
        return Variant.From(Value);
    }

    public void SetFromVariant(Variant variant)
    {
        Value = variant.AsBool();
        Field.SetValue(null, Value);
    }

    public Control CreateEditorItem(LiveValuesModel model)
    {
        var editor = GD.Load<PackedScene>("res://util/live_values/switch_item.tscn").Instantiate<LVSwitchItem>();
        editor.Connect(model, this);
        return editor;
    }
}