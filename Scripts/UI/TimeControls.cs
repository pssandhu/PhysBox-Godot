using Godot;
using System;

public class TimeControls : Control {

    private Button realtimeCheckBox;
    private Label realtimeStepLabel;
    private Slider realtimeStepSlider;
    private Label stepFrequencyLabel;
    private Slider stepFrequencySlider;
    private Label timestepLabel;
    private Slider timestepSlider;

    [Export] private NodePath simulationControllerPath;
    private ISimulationController simulationController;

    public bool Realtime {
        get {
            return realtimeCheckBox.Pressed;
        }
    }

    public override void _Ready() {
        realtimeCheckBox = GetChild<Button>(0);
        realtimeStepLabel = GetChild<Label>(1);
        realtimeStepSlider = GetChild<Slider>(2);
        stepFrequencyLabel = GetChild<Label>(3);
        stepFrequencySlider = GetChild<Slider>(4);
        timestepLabel = GetChild<Label>(5);
        timestepSlider = GetChild<Slider>(6);
        simulationController = GetNode<ISimulationController>(simulationControllerPath);

        realtimeCheckBox.Connect("pressed", this, "RealtimeToggled");

        realtimeCheckBox.Connect("toggled", this, "UpdateTimeSettings");
        realtimeStepSlider.Connect("value_changed", this, "UpdateTimeSettings");
        stepFrequencySlider.Connect("value_changed", this, "UpdateTimeSettings");
        timestepSlider.Connect("value_changed", this, "UpdateTimeSettings");

        // Dud value as the method called by a signal must match its arguments
        // This is the same reason toggled is used as the signal from the CheckBox rather than pressed
        UpdateTimeSettings(1f);
    }

    private void RealtimeToggled() {
        if (realtimeCheckBox.Pressed) {
            realtimeStepLabel.Visible = true;
            realtimeStepSlider.Visible = true;
            stepFrequencyLabel.Visible = false;
            stepFrequencySlider.Visible = false;
            timestepLabel.Visible = false;
            timestepSlider.Visible = false;
        } else {
            realtimeStepLabel.Visible = false;
            realtimeStepSlider.Visible = false;
            stepFrequencyLabel.Visible = true;
            stepFrequencySlider.Visible = true;
            timestepLabel.Visible = true;
            timestepSlider.Visible = true;
        }
    }

    private void UpdateTimeSettings(float value) {
        if (realtimeCheckBox.Pressed) {
            simulationController.Timestep = realtimeStepSlider.Value / 1000;
            Engine.IterationsPerSecond = (int)(1 / (realtimeStepSlider.Value / 1000));
        } else {
            simulationController.Timestep = timestepSlider.Value / 1000;
            Engine.IterationsPerSecond = (int)stepFrequencySlider.Value;
        }
    }

}
