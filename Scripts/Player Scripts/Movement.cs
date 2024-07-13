using Godot;
using System;

public partial class Movement : CharacterBody2D
{
	public int Speed {get; set;} = 150;
	public float WalkMultiplier = 0.5f;

	public double FireDelay {get; set;} = 0.2;

	private PackedScene Bullet = (PackedScene)ResourceLoader.Load("res://bullet.tscn");

	private Timer ShootTimer;
	private Boolean CanShoot = true;

	Random rand = new Random();

	private int Ammo = 30;

	private Godot.Label GunLabel;
	private Godot.Label AmmoLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ShootTimer = GetNode<Node2D>("Player").GetNode<Timer>("ShotTimer");
		ShootTimer.Connect(Timer.SignalName.Timeout, Callable.From(() => SetShoot(true)));

		GunLabel = GetNode<Camera2D>("Camera2D").GetNode<Godot.Label>("GunLabel");
		GunLabel.Text = "AK-47";
		AmmoLabel = GetNode<Camera2D>("Camera2D").GetNode<Godot.Label>("BulletLabel");
		AmmoLabel.Text = Ammo.ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;

		WalkMultiplier = 1;

		if (Input.IsActionPressed("mov_right")) {
			velocity.X += 1;
		}
		if (Input.IsActionPressed("mov_left")) {
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("mov_up")) {
			// + Y is down for some reason
			velocity.Y -= 1;
		}
		if (Input.IsActionPressed("mov_down")) {
			velocity.Y += 1;
		}
		if (Input.IsActionPressed("walk")) {
			WalkMultiplier = 0.3f;
		}

		if (velocity.Length() > 0) {
			velocity = velocity.Normalized() * Speed * WalkMultiplier;

			Velocity = velocity * (float)delta;
			MoveAndCollide(Velocity);
		}

		if (Input.IsActionJustPressed("fire") || Input.IsActionPressed("fire")) {
			if (CanShoot) {
				Shoot();
				Ammo--;
				AmmoLabel.Text = Ammo.ToString();
				SetShoot(false);
				ShootTimer.Start();

				if (Ammo == 0) {
					SetShoot(false);
					ShootTimer.Set("wait_time", 3);
					ShootTimer.Start();
					AmmoLabel.Text = "Reloading...";
				}
			}
		}
	}

	private void Shoot() {
		var VarianceX = (rand.NextInt64() % 8) - 4;
		var VarianceY = (rand.NextInt64() % 8) - 4;

		Vector2 VarianceVect = new Vector2(VarianceX, VarianceY);

		Vector2 Target = GetGlobalMousePosition();

		var BulletInstance = Bullet.Instantiate();
		GetParent().AddChild(BulletInstance);

		Vector2 TargetDirection = Target - GetNode<Node2D>("Player").GetNode<Node2D>("Muzzle").GlobalPosition;
		TargetDirection = TargetDirection.Normalized() * 100;
		TargetDirection = TargetDirection + VarianceVect;

		BulletInstance.Set("position", GetNode<Node2D>("Player").GetNode<Node2D>("Muzzle").GlobalPosition);
		BulletInstance.Set("DirectionalVector", TargetDirection);
		BulletInstance.Set("rotation", GetNode<Node2D>("Player").Rotation);
	}

	private void SetShoot(Boolean shoot) {
		CanShoot = shoot;
		ShootTimer.Stop();

		if ((double)ShootTimer.Get("wait_time") != FireDelay) {
			ShootTimer.Set("wait_time", FireDelay);
			Ammo = 30;
			AmmoLabel.Text = Ammo.ToString();
		}
	}
}
