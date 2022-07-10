using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using Godot.Collections;

/// <summary>
/// The data model for live values.
/// </summary>
public class LiveValuesModel
{
    private const string DATA_PATH = "res://livevalues.json";
    
    private MultiDictionary<string, LiveValue> _valueCategories = new MultiDictionary<string, LiveValue>();

    public LiveValuesModel()
    {
        DiscoverValues();
        LoadJSON(false);
    }

    public IEnumerable<string> Categories => _valueCategories.Keys;

    public IEnumerable<LiveValue> GetValues(string category)
    {
        return _valueCategories[category];
    }

    public LiveValue GetValue(string category, string variable)
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
    
    private void DiscoverValues()
    {
        Assembly lib = typeof(LiveValuesModel).Assembly;
        foreach (Type type in lib.GetTypes())
        {
            foreach (FieldInfo field in type.GetFields())
            {
                LiveValue attrib = field.GetCustomAttribute<LiveValue>();
                if (attrib != null)
                {
                    GD.Print($"Loading field {field.Name} of {type.Name}");
                    attrib.VariableName = field.Name;
                    attrib.Field = field;
                    object value = field.GetValue(null);
                    if (value is float f)
                    {
                        attrib.FloatVal = f;
                    }
                    if (value is bool b)
                    {
                        attrib.BoolVal = b;
                    }
                    _valueCategories.Add(attrib.Category, attrib);
                }
            }
        }
    }

    public void LoadJSON(bool onlyUnmodified)
    {
        File file = new File();
        if (file.FileExists(DATA_PATH))
        {
            file.Open(DATA_PATH, File.ModeFlags.Read);
            string jsonText = file.GetAsText();
            var parsed = JSON.Parse(jsonText);
            if (parsed.Result is Dictionary dict)
            {
                foreach (var category in dict.Keys)
                {
                    var categoryDict = dict[category] as Dictionary;
                    foreach (var variable in categoryDict.Keys)
                    {
                        LiveValue val = GetValue(category.ToString(), variable.ToString());
                        if (val != null)
                        {
                            if (categoryDict[variable] is float f)
                            {
                                if (!onlyUnmodified || val.LastLoadFloatVal != f)
                                {
                                    if (onlyUnmodified)
                                        GD.Print($"Loading modified {val.VariableName} old {val.LastLoadFloatVal} new {f}");
                                    val.FloatVal = f;
                                    val.Field.SetValue(null, f);
                                }
                                val.LastLoadFloatVal = f;
                            }

                            if (categoryDict[variable] is bool b)
                            {
                                if (!onlyUnmodified || val.LastLoadBoolVal != b)
                                {
                                    val.BoolVal = b;
                                    val.Field.SetValue(null, b);
                                }
                                val.LastLoadBoolVal = b;
                            }
                        }
                    }
                }
            }
        }
        file.Close();
    }

    public void SaveJSON()
    {
        if (_valueCategories != null)
        {
            File file = new File();
            file.Open(DATA_PATH, File.ModeFlags.Write);
            Dictionary dict = new Dictionary();
            foreach (var category in _valueCategories.Keys)
            {
                Dictionary categoryDict = new Dictionary();
                foreach (var variable in _valueCategories[category])
                {
                    switch (variable.Type)
                    {
                        case LVType.RANGE:
                            categoryDict[variable.VariableName] = variable.FloatVal;
                            variable.LastLoadFloatVal = variable.FloatVal;
                            break;
                        case LVType.SWITCH:
                            categoryDict[variable.VariableName] = variable.BoolVal;
                            variable.LastLoadBoolVal = variable.BoolVal;
                            break;
                    }
                }

                dict[category] = categoryDict;
            }
            file.StoreLine(JSON.Print(dict, "  "));
            file.Close();
        }
    }
}
