using Godot;
using System;

public class StartMenu : Control
{
    private Button startButton;
    private Button quitButton;

    public override void _Ready()
    {
        startButton = GetNode<Button>("PlayButton");
        quitButton = GetNode<Button>("QuitButton");

        startButton.Connect("pressed", this, nameof(OnPlayButtonPressed));
        quitButton.Connect("pressed", this, nameof(OnQuitButtonPressed));
    }

    public async void OnPlayButtonPressed()
    {
        var tween = CreateTween();
        tween.TweenProperty(startButton, "rect_scale", new Vector2(0.75f, 0.75f), 0.15f).SetTrans(Tween.TransitionType.Quad);
        tween.TweenProperty(startButton, "rect_scale", new Vector2(1f, 1f), 0.15f).SetTrans(Tween.TransitionType.Quad);
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");

        GetTree().ChangeScene("Main.tscn");
    }

    public async void OnQuitButtonPressed()
    {
        var tween = CreateTween();
        tween.TweenProperty(quitButton, "rect_scale", new Vector2(0.75f, 0.75f), 0.15f).SetTrans(Tween.TransitionType.Quad);
        tween.TweenProperty(quitButton, "rect_scale", new Vector2(1f, 1f), 0.15f).SetTrans(Tween.TransitionType.Quad);
        await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        
        GetTree().Quit();
    }

}
