using Godot;
using System;

public partial class player : Node2D
{
	public int Speed {get; set;} = 150;
	public float WalkMultiplier = 0.5f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Rotation
		LookAt(GetGlobalMousePosition());
	}
}