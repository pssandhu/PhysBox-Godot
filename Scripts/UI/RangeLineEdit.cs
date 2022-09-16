using Godot;

public class RangeLineEdit : Node {

    [Export] private NodePath targetRangePath;
    private Range targetRange;
    private LineEdit lineEdit;

    public override void _Ready() {
        lineEdit = (LineEdit)GetParent();
        targetRange = (Range)GetNode(targetRangePath);
        lineEdit.Connect("focus_exited", this, "ValidateInput");
        targetRange.Connect("value_changed", this, "UpdateFromRange");
        UpdateFromRange(targetRange.Value);
    }

    private void ValidateInput() {
        GD.Print("Setting range value to: " + double.Parse(lineEdit.Text));
        targetRange.Set("value", double.Parse(lineEdit.Text));
        // Use the range to validate the new value
        GD.Print("Range set its value to: " + targetRange.Value);
        targetRange.EmitSignal("value_changed", targetRange.Value);
    }

    private void UpdateFromRange(double value) {
        lineEdit.Set("text", value.ToString());
        lineEdit.EmitSignal("text_changed", lineEdit.Text);
    }

}
