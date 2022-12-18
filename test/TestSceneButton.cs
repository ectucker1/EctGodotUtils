using Godot;
using System;

public class TestSceneButton : Button
{
    public override void _Ready()
    {
        base._Ready();

        Connect(SignalNames.BUTTON_PRESSED, this, nameof(_Pressed));
    }

    private void _Pressed()
    {
        Transition.TransitionTo(Text);
    }
}
