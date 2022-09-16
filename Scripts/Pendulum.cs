using Godot;
using System;
using static PhysBox.Constants;
using static PhysBox.Utils;

public class Pendulum : Spatial, ISimulationController {

    private CSGCylinder rod;
    private CSGSphere sphere;

    [Export] private NodePath initialPositionSliderPath;
    [Export] private NodePath lengthSliderPath;
    [Export] private NodePath massSliderPath;
    [Export] private NodePath dampingSliderPath;
    [Export] private NodePath smallAngleCheckBoxPath;
    [Export] private NodePath startStopPath;
    [Export] private NodePath measuredPeriodLabelPath;
    [Export] private NodePath timeControlsPath;

    private Slider initialPositionSlider;
    private Slider lengthSlider;
    private Slider massSlider;
    private Slider dampingSlider;
    private Button smallAngleCheckBox;
    private StartStopButtons startStop;
    private Label measuredPeriodLabel;
    private TimeControls timeControls;

    private bool simulationActive = false;

    [Signal] public delegate void SimulationStarted();
    [Signal] public delegate void SimulationStopped();

    public double Timestep {
        get {
            return timestep;
        }
        set {
            timestep = value;
        }
    }

    // TODO: Improve naming convention
    private double timestep;

    private double theta;
    private double velocity;
    private double acceleration;

    private double measuredPeriod;
    private bool stopwatchActive;
    private int initialVelocitySign;

    // Length of rod in metres
    // To change default, min, and max length change the slider properties in the inspector
    private double length;

    // Initial position of pendulum in degrees
    // To change default, min, and max initial position change the slider properties in the inspector
    private float initialPosition;

    // Pendulum mass in kg
    private double mass;
    private double damping;

    public override void _Ready() {
        rod = GetChild<CSGCylinder>(0);
        sphere = GetChild<CSGSphere>(1);

        initialPositionSlider = GetNode<Slider>(initialPositionSliderPath);
        lengthSlider = GetNode<Slider>(lengthSliderPath);
        massSlider = GetNode<Slider>(massSliderPath);
        dampingSlider = GetNode<Slider>(dampingSliderPath);
        smallAngleCheckBox = GetNode<Button>(smallAngleCheckBoxPath);
        startStop = GetNode<StartStopButtons>(startStopPath);
        measuredPeriodLabel = GetNode<Label>(measuredPeriodLabelPath);
        timeControls = GetNode<TimeControls>(timeControlsPath);

        // Set max damping to less than the lowest damping needed for critical damping based on
        // the range of simulation parameters. Also round it to match the slider Step
        double lowestCriticalDamping = 2 * massSlider.MinValue * Math.Sqrt(g / lengthSlider.MaxValue);
        dampingSlider.MaxValue = Math.Round((lowestCriticalDamping - dampingSlider.Step) / dampingSlider.Step) * dampingSlider.Step;

        SetLength((float)lengthSlider.Value);
        SetMass((float)massSlider.Value);
        SetDamping((float)dampingSlider.Value);
        SetInitialPosition((float)initialPositionSlider.Value);

        // Connect to signals. Can do this in the editor if desired
        initialPositionSlider.Connect("value_changed", this, "SetInitialPosition");
        lengthSlider.Connect("value_changed", this, "SetLength");
        massSlider.Connect("value_changed", this, "SetMass");
        dampingSlider.Connect("value_changed", this, "SetDamping");
        startStop.StartButton.Connect("pressed", this, "StartSimulation");
        startStop.StopButton.Connect("pressed", this, "StopSimulation");
    }

    public override void _PhysicsProcess(float delta) {
        if (simulationActive) {
            if (timeControls.Realtime) {
                // Get current deltaTime
                timestep = delta;
            }

            if (smallAngleCheckBox.Pressed) {
                 acceleration = -g / length * theta - damping / mass * velocity;
            } else {
                acceleration = -g / length * Math.Sin(theta) - damping / mass * velocity;
            }

            velocity += acceleration * timestep;
            double deltaTheta = velocity * timestep;
            theta += deltaTheta;
            RotateZ((float)deltaTheta);

            if (stopwatchActive) {
                GD.Print("timestep: " + timestep);
                GD.Print("delta: " + delta);
                // GD.Print("Velocity: " + velocity);
                measuredPeriod += timestep * 2;

                // Simulation is not accurate enough for velocity to reach exactly zero so check for reversal in velocity direction
                if ((int)(velocity/Math.Abs(velocity)) != initialVelocitySign) {
                    stopwatchActive = false;
                    // Subtract on average half a timestep to account for when the velocity was actually zero
                    measuredPeriod -= 0.5 * timestep;
                    GD.Print("Measured period: " + measuredPeriod);    
                }

                measuredPeriodLabel.Text = "Measured Period: " + measuredPeriod.ToString("0.0000") + " s";
            }
        }
    }

    private void StartSimulation() {
        theta = ToRad(initialPosition);
        velocity = 0;
        measuredPeriod = 0;
        initialVelocitySign = -1 * (int)(initialPosition/Mathf.Abs(initialPosition));
        stopwatchActive = true;
        EmitSignal("SimulationStarted");
        simulationActive = true;
    }

    private void StopSimulation() {
        simulationActive = false;
        stopwatchActive = false;
        RotationDegrees = new Vector3(0, 0, initialPosition);
        EmitSignal("SimulationStopped");
    }

    public void SetInitialPosition(float value) {
        initialPosition = value;
        RotationDegrees = new Vector3(0, 0, value);
    }

    public void SetLength(float value) {
        length = value;
        rod.Height = value;

        Vector3 temp = rod.Translation;
        temp.y = -1 * value / 2;
        rod.Translation = temp;

        temp = sphere.Translation;
        temp.y = -1 * (value + sphere.Radius);
        sphere.Translation = temp;
    }

    public void SetMass(float value) {
        mass = value;
    }

    public void SetDamping(float value) {
        damping = value;
    }

}
