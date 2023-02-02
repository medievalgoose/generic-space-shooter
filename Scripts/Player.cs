using Godot;
using System;

public class Player : KinematicBody2D
{

    [Export]
    public float playerSpeed = 650;
    
    [Export]
    public int bulletSpeed = 2000;

    public bool canShoot = true;
    public bool canMove = true;

    public override void _Ready()
    {
        Timer timer = (Timer) GetNode("Timer");
        timer.Connect("timeout", this, nameof(OnTimerTimeout));
    }

    public override void _PhysicsProcess(float delta)
    {
        PlayerControl();
        Fire();
        PointMouseCursor();
    }

    public void PlayerControl()
    {
        
        var player = GetNode<KinematicBody2D>(".");

        var direction = new Vector2();

        // Cleaner code.
        direction.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        direction.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

        if (canMove)
        {
            MoveAndSlide(direction * playerSpeed);
        }
    }

    public void Fire()
    {
        if (Input.IsActionPressed("click") && canShoot && canMove)
        {
            canShoot = false;

            Timer timer = (Timer) GetNode("Timer");
            timer.Start();

            var bulletInstance = GD.Load<PackedScene>("Scenes/Bullet.tscn").Instance<Bullet>();

            bulletInstance.Position = GetNode<Position2D>("Position2D").GlobalPosition;
            bulletInstance.Rotation = RotationDegrees;
            bulletInstance.ApplyImpulse(new Vector2(), new Vector2(bulletSpeed, 0).Rotated(Rotation));
            bulletInstance.AddToGroup("bullets");
            GetTree().Root.AddChild(bulletInstance);

            // Animate squash as the player fires.
            var tween = CreateTween();
            tween.TweenProperty(this, "scale", new Vector2(0.75f, 0.75f), 0.25f).SetTrans(Tween.TransitionType.Quad);
            tween.TweenProperty(this, "scale", new Vector2(1f, 1f), 0.2f).SetTrans(Tween.TransitionType.Quad);
        }
    }

    public void PointMouseCursor()
    {
        LookAt(GetGlobalMousePosition());
    }

    public void OnTimerTimeout()
    {
        canShoot = true;
    }
}
