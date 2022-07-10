#if TOOLS
using Godot;
using Godot.Collections;

/// <summary>
/// The plugin used to show live values in the editor.
/// </summary>
[Tool]
public class LiveValuesPlugin : EditorPlugin
{
    private const string MAIN_PANEL_PATH = "res://addons/live_values/live_values_main.tscn";
    private const string SWITCH_PATH = "res://addons/live_values/switch_item.tscn";
    private const string SLIDER_NUMBOX_PATH = "res://addons/live_values/slider_numbox.tscn";
    
    private LiveValuesModel _model;
    
    private Control _mainScreen;
    private Tree _categoryTree;
    private Control _valueContainer;

    private string _lastCategory = "";

    private float _lastSave = Mathf.Inf;
    
    public override void _EnterTree()
    {
        if (Engine.EditorHint)
        {
            _model = new LiveValuesModel();
            var mainPanel = GD.Load<PackedScene>(MAIN_PANEL_PATH);
            _mainScreen = mainPanel.Instance<Control>();
            GetEditorInterface().GetEditorViewport().AddChild(_mainScreen);

            _categoryTree = _mainScreen.FindNode("Categories") as Tree;
            _valueContainer = _mainScreen.FindNode("Values") as Control;

            _categoryTree.CreateItem();
            _categoryTree.HideRoot = true;
            _categoryTree.Connect("item_selected", this, nameof(_ItemSelected));

            Init(_model);

            MakeVisible(false);
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
                Control sliderNumbox = GD.Load<PackedScene>(SLIDER_NUMBOX_PATH).Instance<Control>();
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
                _numbox.Connect("value_changed", this, nameof(_ValueChanged), new Array() {_slider});
                _slider.Connect("value_changed", this, nameof(_ValueChanged), new Array() {_numbox});
                _valueContainer.AddChild(sliderNumbox);
                break;
            case LVType.SWITCH:
                Button switchButton = GD.Load<PackedScene>(SWITCH_PATH).Instance<Button>();
                switchButton.Name = value.VariableName;
                switchButton.Pressed = value.BoolVal;
                _valueContainer.AddChild(switchButton);
                break;
        }
    }

    private void _ValueChanged(float value, Range other)
    {
        other.Value = value;
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
                            val.BoolVal = button.Pressed;
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
                            button.Pressed = val.BoolVal;
                        if (node.FindChild<SpinBox>() is SpinBox spinBox)
                            spinBox.Value = val.FloatVal;
                    }
                }
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (Engine.EditorHint)
        {
            _lastSave += delta;
            if (_lastSave > 1.0f)
            {
                CopyGUIValuesToModel();
                _model.LoadJSON(true);
                _model.SaveJSON();
                CopyModelValuesToGUI();
                _lastSave = 0;
            }
        }
    }

    public override void _ExitTree()
    {
        if (_mainScreen != null)
            _mainScreen.QueueFree();
    }

    public override bool HasMainScreen()
    {
        return true;
    }

    public override void MakeVisible(bool visible)
    {
        if (_mainScreen != null)
            _mainScreen.Visible = visible;
    }

    public override string GetPluginName()
    {
        return "LiveValues";
    }

    public override Texture GetPluginIcon()
    {
        return GetEditorInterface().GetBaseControl().GetIcon("ResourcePreloader", "EditorIcons");
    }
}
#endif
