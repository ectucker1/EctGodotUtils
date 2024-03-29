using Godot;
using System;
using System.Collections.Generic;

public partial class InWorldOverlay : Node2D
{
    public static List<IDebugDrawable> Drawables = new List<IDebugDrawable>();
    public static InWorldOverlay Instance;

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    public override void _Draw()
    {
        base._Draw();

        foreach (var drawable in Drawables)
        {
            drawable.Draw(this);
        }
        Drawables.Clear();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        Visible = DebugLayer.Shown;
    }
}
