using System;
using Godot;

public partial class AudioSettings : Node
{
    private const string FILE_PATH = "user://settings.ini";
    private const string SECTION = "audio";

    private const string MAIN_VOLUME_SETTING = "main_volume";
    private const string EFFECTS_VOLUME_SETTING = "effects_volume";
    private const string MUSIC_VOLUME_SETTINGS = "music_volume";
    
    private static AudioSettings _instance;

    private float _mainVolume = 50.0f;
    public static float MainVolume
    {
        get => _instance._mainVolume;
        set
        {
            _instance._mainVolume = Mathf.Clamp(value, 0.0f, 100.0f);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), Mathf.LinearToDb(_instance._mainVolume / 100f));
            _instance._lastUpdated = 0.0f;
            _instance._unsaved = true;
        }
    }
    
    private float _effectsVolume = 100.0f;
    public static float EffectsVolume
    {
        get => _instance._effectsVolume;
        set
        {
            _instance._effectsVolume = Mathf.Clamp(value, 0.0f, 100.0f);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Effects"), Mathf.LinearToDb(_instance._effectsVolume / 100f));
            _instance._lastUpdated = 0.0f;
            _instance._unsaved = true;
        }
    }
    
    private float _musicVolume = 100.0f;
    public static float MusicVolume
    {
        get => _instance._musicVolume;
        set
        {
            _instance._musicVolume = Mathf.Clamp(value, 0.0f, 100.0f);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), Mathf.LinearToDb(_instance._musicVolume / 100f));
            _instance._lastUpdated = 0.0f;
            _instance._unsaved = true;
        }
    }

    private bool _unsaved = false;
    private double _lastUpdated = Mathf.Inf;

    public override void _Ready()
    {
        base._Ready();
        
        _instance = this;
        Load();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_unsaved)
        {
            _lastUpdated += delta;
            if (_lastUpdated >= 1.0f)
            {
                Save();
            }
        }
    }

    private void Save()
    {
        try
        {
            if (OS.GetName() != "HTML5")
            {
                var config = new ConfigFile();
                config.SetValue(SECTION, MAIN_VOLUME_SETTING, _mainVolume);
                config.SetValue(SECTION, EFFECTS_VOLUME_SETTING, _effectsVolume);
                config.SetValue(SECTION, MUSIC_VOLUME_SETTINGS, _musicVolume);
                config.Save(FILE_PATH);
            }
        }
        catch { }

        _unsaved = false;
    }

    private void Load()
    {
        try
        {
            if (OS.GetName() != "HTML5")
            {
                var config = new ConfigFile();
                var error = config.Load(FILE_PATH);
                if (error == Error.Ok)
                {
                    MainVolume = Convert.ToSingle(config.GetValue(SECTION, MAIN_VOLUME_SETTING, 50.0f));
                    EffectsVolume = Convert.ToSingle(config.GetValue(SECTION, EFFECTS_VOLUME_SETTING, 100.0f));
                    MusicVolume = Convert.ToSingle(config.GetValue(SECTION, MUSIC_VOLUME_SETTINGS, 100.0f));
                }
            }
        }
        catch { }
    }
}
