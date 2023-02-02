using Godot;
using System;

public class Enemy : KinematicBody2D
{
	[Signal]
	public delegate void EnemyDestroyed();

	public bool canMove = true;
	public override void _Ready()
	{
		Area2D hitbox = GetNode<Area2D>("Hitbox");
		hitbox.Connect("body_entered", this, nameof(OnBodyEntered));
		hitbox.Connect("mouse_entered", this, nameof(OnMouseEntered));
		hitbox.Connect("mouse_exited", this, nameof(OnMouseExited));
	}

	public async void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("bullets"))
		{
			body.QueueFree();
			EmitSignal("EnemyDestroyed");

			canMove = false;

			var collision = GetNode<CollisionShape2D>("CollisionShape2D");
			
			// Modifying disabled attribute directly from the node doesn't affect the collision status,
			// so I have to use SetDeferred.
			collision.SetDeferred("disabled", true);

			var hitbox = GetNode<Area2D>("Hitbox");
			hitbox.SetDeferred("monitorable", false);
			hitbox.SetDeferred("monitoring", false);

			var tween = CreateTween();
			
			tween.TweenProperty(this, "scale", new Vector2(0.0f, 0.0f), 0.25f).SetTrans(Tween.TransitionType.Quad);
			tween.Parallel().TweenProperty(this, "modulate", new Color("#FF0000"), 0.25f);

			await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
			
			Visible = false;
		} 
	}

	public override void _PhysicsProcess(float delta)
	{
		if (IsInsideTree() && canMove)
		{
			Player player = GetTree().Root.GetNode<Node2D>("Node2D").GetNodeOrNull<Player>("Player");
			var direction = GlobalPosition.DirectionTo(player.GlobalPosition);
			MoveAndSlide(direction.Normalized() * 300);
		}
	}

	public void OnMouseEntered()
	{
		var cursor = GetTree().Root.GetNode<Main>("Node2D").GetNode<Cursor>("Cursor");
		cursor.ChangeCursor();
	}

	public void OnMouseExited()
	{
		var cursor = GetTree().Root.GetNode<Main>("Node2D").GetNode<Cursor>("Cursor");
		cursor.ChangeCursor();
	}
}
