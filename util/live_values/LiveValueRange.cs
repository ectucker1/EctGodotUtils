using System;
using System.Reflection;
using Swan.Formatters;

/// <summary>
/// Attribute for floating point live values in a range.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public partial class LiveValueRange : Attribute, ILiveValue
{
    [JsonProperty("name")]
    public string VariableName{ get; set; } = "Var";
    
    [JsonProperty("category")]
    public string Category { get; set; }
    
    [JsonProperty("min")]
    public float Min { get; set; }
    
    [JsonProperty("max")]
    public float Max { get; set; }
    
    [JsonProperty("value")]
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
}
