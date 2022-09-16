using Godot;

public class StartStopButtons : Control {

    public Button StartButton;
    public Button StopButton;

    public override void _Ready() {
        StartButton = (Button)GetChild(0);
        StopButton = (Button)GetChild(1);
        StartButton.Connect("pressed", this, "StartPressed");
        StopButton.Connect("pressed", this, "StopPressed");
    }

    private void StartPressed() {
        StartButton.Visible = false;
        StopButton.Visible = true;
    }

    private void StopPressed() {
        StopButton.Visible = false;
        StartButton.Visible = true;
    }

}
