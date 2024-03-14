using Godot;

public partial class LiveValuesOverlay : Node
{
    private const string Url = "http://localhost:9696/";
    
    private LiveValuesModel _model;
    
    private TabContainer _categoryContainer;

    private double _nextSave = 0.0f;
    
    public override void _Ready()
    {
        base._Ready();

        _model = new LiveValuesModel();
        
        _categoryContainer = FindChild("Categories") as TabContainer;

        PackedScene categoryScene = GD.Load<PackedScene>("res://util/live_values/live_values_section.tscn");
        foreach (string category in _model.Categories)
        {
            Node categorySection = categoryScene.Instantiate();
            categorySection.Name = category;
            _categoryContainer.AddChild(categorySection);
            Node categoryGrid = categorySection.GetChild(0);
            foreach (ILiveValue value in _model.GetValues(category))
            {
                Label label = new Label();
                label.Text = value.VariableName;
                categoryGrid.AddChild(label);
                categoryGrid.AddChild(value.CreateEditorItem(_model));
            }
        }
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