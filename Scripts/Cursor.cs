using Godot;
using System;

public class Cursor : CanvasLayer
{
	private Sprite normal;
	private Sprite red;
	public override void _Ready()
	{
		normal = GetNode<Node2D>("Shapes").GetNode<Sprite>("Normal");
		red = GetNode<Node2D>("Shapes").GetNode<Sprite>("Red");
	}

	public void ChangeCursor()
	{
		normal.Visible = !normal.Visible;
		red.Visible = !red.Visible;
	}
}
