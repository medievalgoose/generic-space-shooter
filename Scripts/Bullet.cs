using Godot;
using System;

public class Bullet : RigidBody2D
{

    public override void _Ready()
    {
        VisibilityNotifier2D vn = GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");
        vn.Connect("screen_exited", this, nameof(OnScreenExited)); 
    }
    public void OnScreenExited()
    {
        QueueFree();
    }
}
