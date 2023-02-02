using Godot;
using System;

public class EndScreen : Control
{
    public Button retryButton;
    public Button quitButton;
    public Label scoreLabel;
    public ScoreManager scoreManager;

    public override void _Ready()
    {
        retryButton = GetNode<TextureRect>("Container").GetNode<Button>("RetryButton");
        quitButton = GetNode<TextureRect>("Container").GetNode<Button>("QuitButton");
        scoreLabel = GetNode<TextureRect>("Container").GetNode<Label>("ScoreLabel");
        scoreManager = GetTree().Root.GetNode<ScoreManager>("ScoreManager");

        retryButton.Connect("pressed", this, nameof(OnRetryButtonPressed));
        quitButton.Connect("pressed", this, nameof(OnQuitButtonPressed));

        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public override void _Process(float delta)
    {
        scoreLabel.Text = scoreManager.score.ToString();
    }

    public void OnRetryButtonPressed()
    {
        scoreManager.score = 0;
        GetTree().ChangeScene("Main.tscn");
    }

    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
