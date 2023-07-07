using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Godot;
using Swan.Formatters;
using SwanJson = Swan.Formatters.Json;

/// <summary>
/// The data model for live values.
/// </summary>
public class LiveValuesModel
{
    private const string DATA_PATH = "res://livevalues.json";
    
    private Dictionary<string, List<ILiveValue>> _valueCategories = new Dictionary<string, List<ILiveValue>>();
    
    public bool Modified { get; private set; }

    public LiveValuesModel()
    {
        DiscoverValues();
        LoadJSON(false);
    }

    public IEnumerable<string> Categories => _valueCategories.Keys;

    public IEnumerable<ILiveValue> GetValues(string category)
    {
        return _valueCategories[category];
    }

    public ILiveValue GetValue(string category, string variable)
    {
        foreach (var value in _valueCategories[category])
        {
            if (value.VariableName == variable)
            {
                return value;
            }
        }

        return null;
    }

    public void MarkModified()
    {
        Modified = true;
    }

    private void DiscoverValues()
    {
        Assembly lib = typeof(LiveValuesModel).Assembly;
        foreach (Type type in lib.GetTypes())
        {
            foreach (FieldInfo field in type.GetFields())
            {
                LiveValueRange rangeValue = field.GetCustomAttribute<LiveValueRange>();
                if (rangeValue != null)
                {
                    GD.Print($"Discovered field {field.Name} of {type.Name}");
                    rangeValue.VariableName = field.Name;
                    rangeValue.Field = field;
                    object value = field.GetValue(null);
                    if (value is float f)
                    {
                        rangeValue.Value = f;
                        AddLiveValue(rangeValue);
                    }
                    else
                    {
                        GD.Print($"Range live-value on non-float field {field.Name} of {type.Name}");
                    }
                }
                
                LiveValueSwitch switchValue = field.GetCustomAttribute<LiveValueSwitch>();
                if (switchValue != null)
                {
                    GD.Print($"Discovered field {field.Name} of {type.Name}");
                    switchValue.VariableName = field.Name;
                    switchValue.Field = field;
                    object value = field.GetValue(null);
                    if (value is bool b)
                    {
                        switchValue.Value = b;
                        AddLiveValue(switchValue);
                    }
                    else
                    {
                        GD.Print($"Switch live-value on non-bool field {field.Name} of {type.Name}");
                    }
                }
            }
        }
    }

    private void AddLiveValue(ILiveValue value)
    {    
        List<ILiveValue> categoryValues;
        if (!_valueCategories.TryGetValue(value.Category, out categoryValues))
        {
            categoryValues = new List<ILiveValue>();
            _valueCategories[value.Category] = categoryValues;
        }
        categoryValues.Add(value);
    }

    public void LoadJSON(bool onlyUnmodified)
    {
        File file = new File();
        if (file.FileExists(DATA_PATH))
        {
            file.Open(DATA_PATH, File.ModeFlags.Read);
            string jsonText = file.GetAsText();
            var parsed = SwanJson.Deserialize(jsonText);
            if (parsed is Dictionary<string, object> categoryDict)
            {
                foreach (var category in categoryDict)
                {
                    if (category.Value is List<object> categoryValues)
                    {
                        foreach (var value in categoryValues)
                        {
                            if (value is Dictionary<string, object> loadedValue)
                            {
                                ILiveValue discovered = GetValue(category.Key, loadedValue["name"].ToString());
                                if (discovered is LiveValueRange discoveredRange && loadedValue["TypeId"].ToString() == "LiveValueRange")
                                {
                                    discoveredRange.Value = Single.Parse(loadedValue["value"].ToString());
                                    discoveredRange.Field.SetValue(null, discoveredRange.Value);
                                }
                                else if (discovered is LiveValueSwitch discoveredSwitch && loadedValue["TypeId"].ToString() == "LiveValueSwitch")
                                {
                                    discoveredSwitch.Value = Boolean.Parse(loadedValue["value"].ToString());
                                    discoveredSwitch.Field.SetValue(null, discoveredSwitch.Value);
                                }
                            }
                        }
                    }
                }
            }
            file.Close();
        }
    }

    public void SaveJSON()
    {
        if (_valueCategories != null)
        {
            File file = new File();
            file.Open(DATA_PATH, File.ModeFlags.Write);
            file.StoreLine(SwanJson.Serialize(_valueCategories));
            file.Close();
        }

        Modified = false;
    }

    public Dictionary<string, List<ILiveValue>> SerializeValueList()
    {
        return _valueCategories;
    }
}
