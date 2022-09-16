using Godot;

public class ActivityButton : Control {

    private Button button;
    [Export(PropertyHint.File, "*.tscn")] private string scenePath = "";
    [Export] private string buttonText = "";
    [Export] private string activityDescription = "";

    public override void _Ready() {
        button = GetChild<Button>(0);
        // TODO: Change button text when changed in inspector
        button.Text = buttonText;
        button.Connect("pressed", MainMenu.Instance, "PreviewActivity", new Godot.Collections.Array {scenePath, activityDescription});
    }

}
