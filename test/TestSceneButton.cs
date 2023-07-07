using Godot;
using System;

public class TestSceneButton : Button
{
    public override void _Pressed()
    {
        Transition.TransitionTo(Text);
    }
}
