using Godot;

public partial class LiveValuesOverlay : Node
{
    private const string Url = "http://localhost:9696/";
    
    private LiveValuesModel _model;
    
    private Tree _categoryTree;
    private Control _valueContainer;

    private string _lastCategory = "";

    private double _nextSave = 0.0f;
    
    public override void _Ready()
    {
        base._Ready();

        _model = new LiveValuesModel();
        
        _categoryTree = FindChild("Categories") as Tree;
        _valueContainer = FindChild("Values") as Control;
        
        _categoryTree.CreateItem();
        _categoryTree.HideRoot = true;
        _categoryTree.ItemSelected += () => DisplayCategory(_categoryTree.GetSelected().GetText(0));
        
        string firstCategory = null;
        foreach (string category in _model.Categories)
        {
            firstCategory ??= category;
            TreeItem item = _categoryTree.CreateItem();
            item.SetText(0, category);
        }
        DisplayCategory(firstCategory);
    }
    
    private void DisplayCategory(string category)
    {
        _model.SaveConfig();
        _model.LoadConfig();
        
        while (_valueContainer.GetChildCount() > 2)
        {
            _valueContainer.RemoveChild(_valueContainer.GetChild(2));
        }

        foreach (ILiveValue value in _model.GetValues(category))
        {
            AddItem(value);
        }

        _lastCategory = category;
    }
    
    private void AddItem(ILiveValue value)
    {
        Label label = new Label();
        label.Text = value.VariableName;
        _valueContainer.AddChild(label);
        _valueContainer.AddChild(value.CreateEditorItem(_model));
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (_model.Modified && _nextSave <= 0.0f)
            _nextSave = 0.5f;
        
        if (_nextSave > 0.0f)
        {
            _nextSave -= delta;
            if (_nextSave <= 0.0f)
            {
                _model.SaveConfig();
            }
        }
    }
}