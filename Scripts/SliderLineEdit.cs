using Godot;
using System;

public class SliderLineEdit : Node {

    // TODO: Generalise to Range
    [Export]
    private NodePath targetSliderPath;
    private Slider targetSlider;
    private LineEdit lineEdit;

    public override void _Ready() {
        lineEdit = (LineEdit)GetParent();
        targetSlider = (Slider)GetNode(targetSliderPath);
        lineEdit.Connect("focus_exited", this, "ValidateInput");
        targetSlider.Connect("value_changed", this, "UpdateFromSlider");
        UpdateFromSlider(targetSlider.Value);
    }

    private void ValidateInput() {
        GD.Print("Setting slider value to: " + double.Parse(lineEdit.Text));
        targetSlider.Set("value", double.Parse(lineEdit.Text));
        // Use the slider to validate the new value
        GD.Print("Slider set its value to: " + targetSlider.Value);
        targetSlider.EmitSignal("value_changed", targetSlider.Value);
    }

    private void UpdateFromSlider(double value) {
        lineEdit.Set("text", value.ToString());
        lineEdit.EmitSignal("text_changed", lineEdit.Text);
    }

}
