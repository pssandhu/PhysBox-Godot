using Godot;

public class SliderControl : HSlider {

    [Export] private NodePath simulationControllerPath;
    private Node simulationController;
    [Export] bool disableDuringSimulation = false;

    public override void _Ready() {
        if (disableDuringSimulation) {
            simulationController = GetNode(simulationControllerPath);
            simulationController.Connect("SimulationStarted", this, "OnSimulationStart");
            simulationController.Connect("SimulationStopped", this, "OnSimulationStop");
        }
    }

    private void OnSimulationStart() {
        Editable = false;
    }

    private void OnSimulationStop() {
        Editable = true;
    }

}
