using Godot;
using System;

public partial class Enemy_Base : CharacterBody2D
{
	[Export]
	public int Speed {get; set;} = 150;
	[Export]
	public int HP {get;set;} = 100;
	public float WalkMultiplier = 0.5f;

	public override void _PhysicsProcess(double delta)
	{
		
	}
}
