using Godot;
using System;
using static PhysBox.Constants;

public class HangingMassOnSpring : Spatial {

    private CSGCylinder spring;
    private CSGSphere sphere;

    [Export] private NodePath initialPositionSliderPath;
    [Export] private NodePath lengthSliderPath;
    [Export] private NodePath massSliderPath;
    [Export] private NodePath dampingSliderPath;
    [Export] private NodePath startStopPath;

    private Slider initialPositionSlider;
    private Slider lengthSlider;
    private Slider massSlider;
    private Slider dampingSlider;
    private StartStopButtons startStop;

    private bool simulationActive = false;

    [Signal] public delegate void SimulationStarted();
    [Signal] public delegate void SimulationStopped();

    private double timestep;

    private double displacement;
    private double velocity;
    private double acceleration;

    // Initial displacement of sping in m
    // To change default, min, and max values change the slider properties in the inspector
    private float initialPosition;

    // Sping constant in N.m^-1
    private double springConstant;

    // Mass in kg
    private double mass;
    private double damping;

    public override void _Ready() {
        spring = GetChild<CSGCylinder>(0);
        sphere = GetChild<CSGSphere>(1);

        initialPositionSlider = GetNode<Slider>(initialPositionSliderPath);
        lengthSlider = GetNode<Slider>(lengthSliderPath);
        massSlider = GetNode<Slider>(massSliderPath);
        dampingSlider = GetNode<Slider>(dampingSliderPath);
        startStop = GetNode<StartStopButtons>(startStopPath);

        // Set max damping?

        springConstant = 10;
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
            timestep = delta;

            acceleration = -g * mass - springConstant * displacement;
            velocity += acceleration * timestep;
            double deltaDisplacement = velocity * timestep;
            displacement += deltaDisplacement;
            MoveSpring(displacement);
        }
    }

    private void StartSimulation() {
        displacement = initialPosition;
        velocity = 0;
        EmitSignal("SimulationStarted");
        simulationActive = true;
    }

    private void StopSimulation() {
        simulationActive = false;
        //MoveSpring(initialPosition);
        EmitSignal("SimulationStopped");
    }

    public void SetInitialPosition(float value) {
        initialPosition = value;
        MoveSpring(value);
    }

    public void MoveSpring(double newDisplacement) {
        spring.Height = (float)Math.Abs(newDisplacement);
        Vector3 temp = spring.Translation;
        temp.y = -1 * spring.Height / 2;
        spring.Translation = temp;
    }

    public void SetMass(float value) {
        mass = value;
    }

    public void SetDamping(float value) {
        damping = value;
    }

}
