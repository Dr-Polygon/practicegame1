using Godot;
using System;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		new_Game();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void onScoreTimerTimeout()
	{
		_score++;
	}

	private void OnStartTimerTImeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

		public void game_Over()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
	}

	public void new_Game()
	{
		_score = 0;

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("startPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
	}

	private void onMobTimerTimeout()
	{
		Mob mob = MobScene.Instantiate<Mob>();

		// choose a random location on path2D
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		// set the mobs direction perpendicular to the path
		float direction = mobSpawnLocation.Rotation = Mathf.Pi / 2;

		// set the mobs position to a random location
		mob.Position = mobSpawnLocation.Position;

		// add some randomness to direction
		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;

		// choose the velocity
		var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);

		// spawn the mob by adding it to the scene
		AddChild(mob);
	}

	[Export]
	public PackedScene MobScene { get; set; }

	private int _score;
}
