using Godot;
using System;

public partial class LVSwitchItem : CheckButton, ILVItem<LiveValueSwitch>
{
    public void Connect(LiveValuesModel model, LiveValueSwitch val)
    {
        ButtonPressed = val.Value;
        Pressed += () =>
        {
            val.SetFromVariant(Variant.From(ButtonPressed));
            model.MarkModified();
        };
    }
}
