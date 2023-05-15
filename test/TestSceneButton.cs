using Godot;
using System;

public partial class TestSceneButton : Button
{
    public override void _Pressed()
    {
        Transition.TransitionTo(Text);
    }
}
