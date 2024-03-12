using System;
using Godot;

public partial class LVRangeItem : Control, ILVItem<LiveValueRange>
{
    public void Connect(LiveValuesModel model, LiveValueRange val)
    {
        HSlider slider = this.FindChild<HSlider>();
        SpinBox numbox = this.FindChild<SpinBox>();
        slider.MinValue = val.Min;
        slider.MaxValue = val.Max;
        slider.Step = 0.000001f;
        slider.Value = val.Value;
        numbox.MinValue = val.Min;
        numbox.MaxValue = val.Max;
        numbox.Step = 0.000001f;
        numbox.Value = val.Value;

        void UpdateValue(double value)
        {
            if (val.Value != value)
            {
                val.SetFromVariant(Variant.From(value));
                model.MarkModified();
            }
        }
            
        numbox.ValueChanged += (double value) =>
        {
            slider.Value = value;
            UpdateValue(value);
        };
        slider.ValueChanged += (double value) =>
        {
            numbox.Value = value;
            UpdateValue(value);
        };
    }
}
