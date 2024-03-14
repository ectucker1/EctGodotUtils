using Godot;

public partial class LVRangeItem : Control, ILVItem<LiveValueRange>
{
    private LiveValuesModel _model;
    private LiveValueRange _val;

    private HSlider _slider;
    private SpinBox _numbox;
    
    private Button _learn;
    private SpinBox _midiMin;
    private SpinBox _midiMax;
    
    private bool _midiLearning = false;
    private Optional<MidiControlChannel> _midiChannel = Optional<MidiControlChannel>.None;

    public override void _Ready()
    {
        base._Ready();
        
        _slider = GetNode<HSlider>("SliderNumbox/Slider");
        _numbox = GetNode<SpinBox>("SliderNumbox/Numbox");

        _learn = GetNode<Button>("MIDI/LearnButton");
        _midiMin = GetNode<SpinBox>("MIDI/MidiZero");
        _midiMax = GetNode<SpinBox>("MIDI/MidiMax");
        
        _slider.MinValue = _val.Min;
        _slider.MaxValue = _val.Max;
        _slider.Step = 0.000001f;
        _slider.Value = _val.Value;
        
        _numbox.MinValue = _val.Min;
        _numbox.MaxValue = _val.Max;
        _numbox.Step = 0.000001f;
        _numbox.Value = _val.Value;
        
        _numbox.ValueChanged += (double value) =>
        {
            _slider.Value = value;
            UpdateValue(value);
        };
        _slider.ValueChanged += (double value) =>
        {
            _numbox.Value = value;
            UpdateValue(value);
        };
        
        _midiMin.MinValue = _val.Min;
        _midiMin.MaxValue = _val.Max;
        _midiMin.Step = 0.000001f;
        _midiMin.Value = _val.Min;
        
        _midiMax.MinValue = _val.Min;
        _midiMax.MaxValue = _val.Max;
        _midiMax.Step = 0.000001f;
        _midiMax.Value = _val.Max;

        _midiChannel = _model.GetMapping(_val);
        _midiChannel.MatchSome(channel =>
        {
            if (OS.GetConnectedMidiInputs().IsEmpty())
                OS.OpenMidiInputs();
            _midiMin.Value = channel.Min;
            _midiMax.Value = channel.Max;
            _midiLearning = false;
            _learn.Text = channel.ToString();
        });

        _midiMin.ValueChanged += _ =>
        {
            if (_midiChannel.IsSome)
            {
                _midiChannel = Optional<MidiControlChannel>.Some(
                    new MidiControlChannel(_midiChannel.Value, (float)_midiMin.Value, (float)_midiMax.Value));
                _model.PutMapping(_val, _midiChannel.Value);
            }
        };
        _midiMax.ValueChanged += _ =>
        {
            if (_midiChannel.IsSome)
            {
                _midiChannel = Optional<MidiControlChannel>.Some(
                    new MidiControlChannel(_midiChannel.Value, (float)_midiMin.Value, (float)_midiMax.Value));
                _model.PutMapping(_val, _midiChannel.Value);
            }
        };

        _learn.Pressed += () =>
        {
            if (!_midiLearning)
            {
                _midiChannel.Match(_ =>
                    {
                        _learn.Text = "LEARN";
                        _midiChannel = Optional<MidiControlChannel>.None;
                        _model.EraseMapping(_val);
                        _midiLearning = false;
                    },
                    () =>
                    {
                        if (OS.GetConnectedMidiInputs().IsEmpty())
                            OS.OpenMidiInputs();
                        _learn.Text = "...";
                        _midiLearning = true;
                    });
            }
            else
            {
                _learn.Text = "LEARN";
                _midiChannel = Optional<MidiControlChannel>.None;
                _model.EraseMapping(_val);
                _midiLearning = false;
            }
        };
    }

    public void Connect(LiveValuesModel model, LiveValueRange val)
    {
        _model = model;
        _val = val;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMidi midiEvent)
        {
            if (_midiLearning && midiEvent.Message == MidiMessage.ControlChange)
            {
                _midiChannel = Optional<MidiControlChannel>.Some(
                    new MidiControlChannel(
                        midiEvent.Channel,
                        midiEvent.ControllerNumber,
                        (float) _midiMin.Value,
                        (float) _midiMax.Value
                    )
                );
                _model.PutMapping(_val, _midiChannel.Value);
                _midiLearning = false;
                _learn.Text = _midiChannel.Value.ToString();
            }
            else
            {
                _midiChannel.MatchSome(channel =>
                {
                    if (channel.Matches(midiEvent))
                    {
                        float value = channel.ScaleValue(midiEvent);
                        _slider.Value = value;
                        _numbox.Value = value;
                        UpdateValue(value);
                    }
                });
            }
        }
    }
    
    private void UpdateValue(double value)
    {
        if (_val.Value != value)
        {
            _val.SetFromVariant(Variant.From(value));
            _model.MarkModified();
        }
    }
}
