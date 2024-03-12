using Godot;

/// <summary>
/// Interface for attributes that describe fields as live values.
/// </summary>
public interface ILiveValue
{
    string VariableName { get; }
    
    string Category { get; }

    public Variant ToVariant();

    public void SetFromVariant(Variant variant);

    public Control CreateEditorItem(LiveValuesModel model);
}
