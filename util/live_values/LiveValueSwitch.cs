using System;
using System.Reflection;
using Swan.Formatters;

/// <summary>
/// Attribute for switch live values.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public partial class LiveValueSwitch : Attribute, ILiveValue
{
    [JsonProperty("name")]
    public string VariableName{ get; set; } = "Var";
    
    [JsonProperty("category")]
    public string Category { get; set; }

    [JsonProperty("value")]
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
}