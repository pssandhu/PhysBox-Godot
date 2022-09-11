using Godot;
using System;
using static PhysBox.Constants;
using static PhysBox.Utils;

public class Pendulum : Spatial {

    private bool simulationActive = false;

    private double timestep;

    private double theta;
    private double velocity;
    private double acceleration;

    // Length of rod in metres
    // To change default, min, and max length change the slider properties in the inspector
    private double length;
    private float sphereRadius;

    // Initial position of pendulum in degrees
    // To change default, min, and max initial position change the slider properties in the inspector
    private float initialPosition;

    // Pendulum mass in kg
    private double mass;
    private double damping;

    public override void _Ready() {
        sphereRadius = 0.125f;
        length = 1.5;
        mass = 1;
        damping = 0.5;
        SetInitialPosition(-150);
        StartSimulation();
    }

    public override void _PhysicsProcess(float delta) {
        if (simulationActive) {
            timestep = delta;

            acceleration = -g / length * Math.Sin(theta) - damping / mass * velocity;

            velocity += acceleration * timestep;
            double deltaTheta = velocity * timestep;
            theta += deltaTheta;
            RotateZ((float)deltaTheta);
        }
    }

    public void StartSimulation() {
        theta = ToRad(initialPosition);
        velocity = 0;
        simulationActive = true;
    }

    public void StopSimulation() {
        simulationActive = false;
        RotationDegrees = new Vector3(0, 0, initialPosition);
    }

    public void SetInitialPosition(float value) {
        initialPosition = value;
        RotationDegrees = new Vector3(0, 0, value);
    }

}
