using Godot;

public class MainMenu : Control {

    public static MainMenu Instance;

    [Export] private NodePath selectActivityLabelPath;
    [Export] private NodePath descriptionLabelPath;
    [Export] private NodePath openButtonPath;
    [Export] private NodePath quitButtonPath;
    private Label SelectActivityLabel;
    private Label DescriptionLabel;
    private Button openButton;
    private Button quitButton;

    private string SelectedActivityScene;

    public override void _EnterTree() {
        Instance = this;
    }

    public override void _Ready() {
        SelectActivityLabel = GetNode<Label>(selectActivityLabelPath);
        DescriptionLabel = GetNode<Label>(descriptionLabelPath);
        openButton = GetNode<Button>(openButtonPath);
        quitButton = GetNode<Button>(quitButtonPath);

        openButton.Connect("pressed", this, "LoadSelectedActivity");
        quitButton.Connect("pressed", this, "QuitApplication");

        if (OS.GetName() == "HTML5") {
            quitButton.Visible = false;
        }

        Engine.IterationsPerSecond = 60;
    }

    public void PreviewActivity(string scenePath, string activityDescription) {
        if (SelectedActivityScene != scenePath) {
            SelectedActivityScene = scenePath;
            DescriptionLabel.Text = activityDescription;
            SelectActivityLabel.Visible = false;
            DescriptionLabel.Visible = true;
            openButton.Visible = true;
        }
    }

    private void LoadSelectedActivity() {
        GD.Print("Loading scene: " + SelectedActivityScene);
        GetTree().ChangeScene(SelectedActivityScene);
    }

    private void QuitApplication() {
        GD.Print("Quitting application...");
        GetTree().Quit();
    }

}
