using Godot;
using System;

public class StartStopButtons : Control {

    public Button startButton;
    public Button stopButton;

    public override void _Ready() {
        startButton = (Button)GetChild(0);
        stopButton = (Button)GetChild(1);
        startButton.Connect("pressed", this, "StartPressed");
        stopButton.Connect("pressed", this, "StopPressed");
    }

    private void StartPressed() {
        startButton.Visible = false;
        stopButton.Visible = true;
    }

    private void StopPressed() {
        stopButton.Visible = false;
        startButton.Visible = true;
    }

}
