using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;

/// <summary>
/// The data model for live values.
/// </summary>
public class LiveValuesModel
{
    private const string DataPath = "res://livevalues.ini";
    private const string MidiPath = "res://livevalues_midi.ini";
    
    private readonly Dictionary<string, List<ILiveValue>> _valueCategories = new();

    private readonly Dictionary<ILiveValue, MidiControlChannel> _mappings = new();
    
    public bool Modified { get; private set; }

    public LiveValuesModel()
    {
        DiscoverValues();
        LoadConfig();
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

    public void PutMapping(ILiveValue value, MidiControlChannel mapping)
    {
        _mappings[value] = mapping;
    }

    public void EraseMapping(ILiveValue value)
    {
        _mappings.Remove(value);
    }
    
    public Optional<MidiControlChannel> GetMapping(ILiveValue value)
    {
        if (_mappings.TryGetValue(value, out var mapping))
        {
            return Optional<MidiControlChannel>.Some(mapping);
        }
        return Optional<MidiControlChannel>.None;
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

    public void LoadConfig()
    {
        if (FileAccess.FileExists(DataPath))
        {
            var config = new ConfigFile();
            if (config.Load(DataPath) == Error.Ok)
            {
                foreach (var category in config.GetSections())
                {
                    foreach (var key in config.GetSectionKeys(category))
                    {
                        ILiveValue discovered = GetValue(category, key);
                        discovered.SetFromVariant(config.GetValue(category, key, discovered.ToVariant()));
                    }
                }
            }

            var midiConfig = new ConfigFile();
            if (midiConfig.Load(MidiPath) == Error.Ok)
            {
                foreach (var mapping in midiConfig.GetSections())
                {
                    string category = midiConfig.GetValue(mapping, "category").AsString();
                    string key = midiConfig.GetValue(mapping, "key").AsString();
                    int channel = midiConfig.GetValue(mapping, "channel").AsInt32();
                    int controller = midiConfig.GetValue(mapping, "cc").AsInt32();
                    float min = midiConfig.GetValue(mapping, "min").AsSingle();
                    float max = midiConfig.GetValue(mapping, "max").AsSingle();
                    foreach (ILiveValue value in _valueCategories[category])
                    {
                        if (value.VariableName.Equals(key))
                        {
                            _mappings[value] = new MidiControlChannel(channel, controller, min, max);
                        }
                    }
                }
            }
        }
    }

    public void SaveConfig()
    {
        var config = new ConfigFile();
        foreach (var category in _valueCategories)
        {
            foreach (var value in category.Value)
            {
                config.SetValue(category.Key, value.VariableName, value.ToVariant());
            }
        }
        config.Save(DataPath);

        var midiConfig = new ConfigFile();
        foreach (var pair in _mappings)
        {
            string mapping = $"{pair.Key.Category}_{pair.Key.VariableName}";
            midiConfig.SetValue(mapping, "category", pair.Key.Category);
            midiConfig.SetValue(mapping, "key", pair.Key.VariableName);
            midiConfig.SetValue(mapping, "channel", pair.Value.Channel);
            midiConfig.SetValue(mapping, "cc", pair.Value.Controller);
            midiConfig.SetValue(mapping, "min", pair.Value.Min);
            midiConfig.SetValue(mapping, "max", pair.Value.Max);
        }
        midiConfig.Save(MidiPath);

        Modified = false;
    }
}
