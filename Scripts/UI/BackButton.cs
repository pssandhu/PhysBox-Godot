using Godot;

public class BackButton : Control {

    public override void _Ready() {
        GetChild<Button>(0).Connect("pressed", this, "LoadMainMenu");
    }

    private void LoadMainMenu() {
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }

}
