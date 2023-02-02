using Godot;
using System;

public class Main : Node2D
{
	public int counter = 0;
	public int level = 0;

	Godot.Collections.Array enemies = new Godot.Collections.Array();

	public Node2D cursor;

	public ScoreManager scoreManager;

	public override void _Ready()
	{
		var timer = GetNode<Timer>("Timer");
		timer.Connect("timeout", this, nameof(OnTimerTimeout));
		timer.Start();

		AnimateCounter();

		Input.MouseMode = Input.MouseModeEnum.Hidden;

		cursor = GetNode<CanvasLayer>("Cursor").GetNode<Node2D>("Shapes");

		scoreManager = GetTree().Root.GetNode<ScoreManager>("/root/ScoreManager");

	}

	public override void _PhysicsProcess(float delta)
	{
		Label scoreLabel = GetNodeOrNull<Label>("Score");
		scoreLabel.Text = "Score: " + scoreManager.score;

		cursor.GlobalPosition = GetGlobalMousePosition();

	}

	public async void OnEnemyDestroyed()
	{
		// score++;
		counter++;

		scoreManager.score++;

		if (counter % (5 + level) == 0)
		{
			counter = 0;

			await ToSignal(GetTree().CreateTimer(1f), "timeout");
			for (int i = 0; i < enemies.Count; i++)
			{
				Enemy enemy = (Enemy) enemies[i];
				enemy.QueueFree();
			}

			enemies.Clear();

			// Replacement for yield.
			await ToSignal(GetTree().CreateTimer(2f), "timeout");
			
			level += 1;
			
			SpawnEnemies();
		}
	}

	public void SpawnEnemies()
	{
		RandomNumberGenerator RNG = new RandomNumberGenerator();
		//PackedScene EnemyScene = (PackedScene) ResourceLoader.Load("Enemy.tscn");

		for (int i = 0; i < 5 + level; i++)
		{   
			//KinematicBody2D enemy = (KinematicBody2D) EnemyScene.Instance();
			// Enemy enemy = (Enemy) EnemyScene.Instance();
			
			var enemy = GD.Load<PackedScene>("Scenes/Enemy.tscn").Instance<Enemy>();

			RNG.Randomize();

			int chooser = RNG.RandiRange(0, 3);

			// Calculate the spawn position.
			int xCoords = RNG.RandiRange(-200, 1350);
			int yCoords = RNG.RandiRange(-100, 675);

			while (xCoords >= 0 && xCoords <= 1280 && yCoords >= 0 && yCoords <= 720)
			{
				RNG.Randomize();
				xCoords = RNG.RandiRange(-150, 1250);
				yCoords = RNG.RandiRange(-50, 700);
			}

			enemy.Position = new Vector2(xCoords, yCoords);
			AddChild(enemy);
			enemies.Add(enemy);
		}

		for (int i = 0; i < enemies.Count; i++)
		{
			Enemy enemy = (Enemy) enemies[i];
			enemy.Connect("EnemyDestroyed", this, nameof(OnEnemyDestroyed));
		}
	}

	public void DisableEnemyMovement()
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			Enemy enemy = (Enemy) enemies[i];
			
			if (enemy.IsInsideTree())
			{
				enemy.canMove = false;
			}
		}
	}

	public async void OnTimerTimeout()
	{
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		SpawnEnemies();
	}

	public async void AnimateCounter()
	{
		var timer = GetNode<Timer>("Timer");
		var counter = GetNode<Label>("Counter");


		for (int i = (int) Math.Round(timer.TimeLeft); i >= 0; i--)
		{
			if (Math.Round(timer.TimeLeft) == 0)
			{
				counter.Text = "Start!";
			} 
			else
			{
				counter.Text = (Math.Round(timer.TimeLeft)).ToString();
			}

			var tween = CreateTween();
			tween.TweenProperty(counter, "rect_scale", new Vector2(1, 1), 0.5f).SetTrans(Tween.TransitionType.Quad);
			tween.TweenProperty(counter, "rect_scale", new Vector2(0, 0), 0.5f).SetTrans(Tween.TransitionType.Quad);
			await ToSignal(GetTree().CreateTimer(1f), "timeout");
		}
	}
}
