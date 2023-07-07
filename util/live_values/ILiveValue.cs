/// <summary>
/// Interface for attributes that describe fields as live values.
/// </summary>
public interface ILiveValue
{
    string VariableName { get; }
    
    string Category { get; }
}
