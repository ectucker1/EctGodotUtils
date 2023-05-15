#if TOOLS
using System;
using Godot;
using Array = Godot.Collections.Array;

/// <summary>
/// The plugin used to show live values in the editor.
/// </summary>
[Tool]
public partial class LiveValuesPlugin : EditorPlugin
{
    private const string MAIN_PANEL_PATH = "res://addons/live_values/live_values_main.tscn";
    private const string SWITCH_PATH = "res://addons/live_values/switch_item.tscn";
    private const string SLIDER_NUMBOX_PATH = "res://addons/live_values/slider_numbox.tscn";
    
    private LiveValuesModel _model;
    
    private Control _mainScreen;
    private Tree _categoryTree;
    private Control _valueContainer;

    private string _lastCategory = "";

    private long _framesSinceChange = -1;
    
    public override void _EnterTree()
    {
        if (Engine.IsEditorHint())
        {
            _model = new LiveValuesModel();
            var mainPanel = GD.Load<PackedScene>(MAIN_PANEL_PATH);
            _mainScreen = mainPanel.Instantiate<Control>();
            GetEditorInterface().GetEditorMainScreen().AddChild(_mainScreen);

            _categoryTree = _mainScreen.FindChild("Categories") as Tree;
            _valueContainer = _mainScreen.FindChild("Values") as Control;

            _categoryTree.CreateItem();
            _categoryTree.HideRoot = true;
            _categoryTree.Connect("item_selected", new Callable(this, nameof(_ItemSelected)));

            Init(_model);

            _MakeVisible(false);
        }
    }
    
    public void Init(LiveValuesModel model)
    {
        _model = model;
        string firstCategory = null;
        foreach (string category in model.Categories)
        {
            if (firstCategory == null)
                firstCategory = category;
            TreeItem item = _categoryTree.CreateItem();
            item.SetText(0, category);
        }
        DisplayCategory(firstCategory);
    }

    public void DisplayCategory(string category)
    {
        CopyGUIValuesToModel();
        _model.LoadJSON(true);
        _model.SaveJSON();
        
        while (_valueContainer.GetChildCount() > 2)
        {
            _valueContainer.RemoveChild(_valueContainer.GetChild(2));
        }

        foreach (LiveValue value in _model.GetValues(category))
        {
            AddItem(value);
        }

        _lastCategory = category;
    }

    public void AddItem(LiveValue value)
    {
        Label label = new Label();
        label.Text = value.VariableName;
        _valueContainer.AddChild(label);
        switch (value.Type)
        {
            case LVType.RANGE:
                Control sliderNumbox = GD.Load<PackedScene>(SLIDER_NUMBOX_PATH).Instantiate<Control>();
                sliderNumbox.Name = value.VariableName;
                Slider _slider = sliderNumbox.FindChild<Slider>();
                SpinBox _numbox = sliderNumbox.FindChild<SpinBox>();
                _slider.MinValue = value.Min;
                _slider.MaxValue = value.Max;
                _slider.Step = 0.000001f;
                _slider.Value = value.FloatVal;
                _numbox.MinValue = value.Min;
                _numbox.MaxValue = value.Max;
                _numbox.Step = 0.000001f;
                _numbox.Value = value.FloatVal;
                _numbox.ValueChanged += (double value) => _ValueChanged(value, _slider);
                _slider.ValueChanged += (double value) => _ValueChanged(value, _numbox);
                _valueContainer.AddChild(sliderNumbox);
                break;
            case LVType.SWITCH:
                Button switchButton = GD.Load<PackedScene>(SWITCH_PATH).Instantiate<Button>();
                switchButton.Name = value.VariableName;
                switchButton.ButtonPressed = value.BoolVal;
                switchButton.Pressed += () => _framesSinceChange = 0;
                _valueContainer.AddChild(switchButton);
                break;
        }
    }

    private void _ValueChanged(double value, Godot.Range other)
    {
        other.Value = value;
        _framesSinceChange = 0;
    }

    private void _ItemSelected()
    {
        DisplayCategory(_categoryTree.GetSelected().GetText(0));
    }

    private void CopyGUIValuesToModel()
    {
        var lastVals = _model?.GetValues(_lastCategory);
        if (lastVals != null && _valueContainer != null)
        {
            foreach (var val in lastVals)
            {
                foreach (Node node in _valueContainer.GetChildren())
                {
                    if (node.Name == val.VariableName)
                    {
                        if (node is Button button)
                            val.BoolVal = button.ButtonPressed;
                        if (node.FindChild<SpinBox>() is SpinBox spinBox)
                            val.FloatVal = (float) spinBox.Value;
                    }
                }
            }
        }
    }

    private void CopyModelValuesToGUI()
    {
        var lastVals = _model?.GetValues(_lastCategory);
        if (lastVals != null && _valueContainer != null)
        {
            foreach (var val in lastVals)
            {
                foreach (Node node in _valueContainer.GetChildren())
                {
                    if (node.Name == val.VariableName)
                    {
                        if (node is Button button)
                            button.ButtonPressed = val.BoolVal;
                        if (node.FindChild<SpinBox>() is SpinBox spinBox)
                            spinBox.Value = val.FloatVal;
                    }
                }
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Engine.IsEditorHint())
        {
            if (_framesSinceChange >= 0)
                _framesSinceChange += 1;
            if (_framesSinceChange >= 3)
            {
                try
                {
                    CopyGUIValuesToModel();
                    _model.LoadJSON(true);
                    _model.SaveJSON();
                    CopyModelValuesToGUI();
                    _framesSinceChange = -1;
                }
                catch (Exception e)
                {
                    GD.PrintErr("Error saving live values. Disabling until plugin refresh.");
                    SetProcess(false);
                    SetPhysicsProcess(false);
                    // TODO maybe I can refresh myself?
                }
            }
        }
    }

    public override void _ExitTree()
    {
        if (_mainScreen != null)
            _mainScreen.QueueFree();
    }

    public override bool _HasMainScreen()
    {
        return true;
    }
    
    public override void _MakeVisible(bool visible)
    {
        if (_mainScreen != null)
            _mainScreen.Visible = visible;
    }

    public override string _GetPluginName()
    {
        return "LiveValues";
    }

    public override Texture2D _GetPluginIcon()
    {
        return GetEditorInterface().GetBaseControl().GetThemeIcon("ResourcePreloader", "EditorIcons");
    }
}
#endif
