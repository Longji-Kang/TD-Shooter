using Godot;
using System;

public partial class bullet : CharacterBody2D
{
	[Export]
	public int MuzzleVelocity = 2000;
	Vector2 DirectionalVector = new Vector2(1, 0);

	private ulong SpawnTime;

	private int LifeSpan = 3000;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpawnTime = Time.GetTicksMsec();
	}

	public override void _PhysicsProcess(double delta)
	{
		this.Velocity = DirectionalVector.Normalized() * MuzzleVelocity * (float)delta;
		KinematicCollision2D Collision = MoveAndCollide(Velocity);

		if (Collision != null) {
			QueueFree();
		} else if (SpawnTime + (ulong)LifeSpan <= Time.GetTicksMsec()) {
			QueueFree();
		}
	}
}
