using Godot;
using System;

/// <summary>
/// A game wide pause menu. Handles showing and hiding using it's own UI action.
/// Also handles the toggle fullscreen action.
/// 
/// Should be disabled on other menus.
/// </summary>
public class PauseMenu : Control
{
    private static PauseMenu _instance;

    private bool _enabled = true;

    private bool _shown = false;

    /// <summary>
    /// Whether or not to enable the pause menu.
    /// </summary>
    public static bool Enabled
    {
        get => _instance._enabled;
        set
        { 
            _instance._enabled = value;
            if (_instance._shown)
                _instance.ToggleShown();
        }
    }

    private Button _resumeButton;
    private Button _exitButton;

    private Range _mainVolumeRange;
    private Range _effectsVolumeRange;
    private Range _musicVolumeRange;

    private AnimationPlayer _animPlayer;
    
    public override void _Ready()
    {
        base._Ready();

        _instance = this;

        _resumeButton = FindNode("Resume") as Button;
        _resumeButton.Connect(SignalNames.BUTTON_PRESSED, this, nameof(ToggleShown));
        
        _exitButton = FindNode("Exit") as Button;
        _exitButton.Connect(SignalNames.BUTTON_PRESSED, this, nameof(Exit));
        if (OS.GetName() == "HTML5")
        {
            _exitButton.Visible = false;
        }

        _animPlayer = this.FindChild<AnimationPlayer>();
        _animPlayer.Connect(SignalNames.ANIMATION_FINISHED, this, nameof(_AnimationFinished));

        _mainVolumeRange = FindNode("MainSlider") as Range;
        _mainVolumeRange.Value = AudioSettings.MainVolume;
        _mainVolumeRange.Connect(SignalNames.RANGE_VALUE_CHANGED, this, nameof(UpdateVolumes));
        _effectsVolumeRange = FindNode("EffectsSlider") as Range;
        _effectsVolumeRange.Value = AudioSettings.EffectsVolume;
        _effectsVolumeRange.Connect(SignalNames.RANGE_VALUE_CHANGED, this, nameof(UpdateVolumes));
        _musicVolumeRange = FindNode("MusicSlider") as Range;
        _musicVolumeRange.Value = AudioSettings.MusicVolume;
        _musicVolumeRange.Connect(SignalNames.RANGE_VALUE_CHANGED, this, nameof(UpdateVolumes));
    }

    public override void _Input(InputEvent inputEvent)
    {
        base._Input(inputEvent);

        if (inputEvent.IsActionPressed("pause"))
        {
            ToggleShown();
        }
        else if (inputEvent.IsActionPressed("fullscreen"))
        {
            OS.WindowFullscreen = !OS.WindowFullscreen;
        }
    }

    public void ToggleShown()
    {
        if (!_animPlayer.IsPlaying())
        {
            if (!_shown)
            {
                _animPlayer.Play("Show");
                GetTree().Paused = true;
                _shown = true;
            }
            else
            {
                _animPlayer.Play("Hide");
            }
        }
    }

    public void Exit()
    {
        GetTree().Quit();
    }

    private void UpdateVolumes(float _)
    {
        AudioSettings.MainVolume = (float)_mainVolumeRange.Value;
        AudioSettings.EffectsVolume = (float)_effectsVolumeRange.Value;
        AudioSettings.MusicVolume = (float)_musicVolumeRange.Value;
    }

    private void _AnimationFinished(string name)
    {
        if (name == "Hide")
        {
            GetTree().Paused = false;
            _shown = false;
        }
    }
}
