using Godot;
using System;

interface ISimulationController {

    double Timestep {
        get;
        set;
    }

    // TODO: Add simulation start and stop signals to interface
    // May require C# 8.0 https://github.com/godotengine/godot/issues/49439#issuecomment-857157446

}
