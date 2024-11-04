using Godot;
using System;

public partial class Player : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; // player movement vector

		if (Input.IsActionPressed("move_Right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_Left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_Down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_Up"))
		{
			velocity.Y -=1;
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		Position += velocity * (float)delta;
		GlobalPosition = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);

		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipV = false;
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		else if (velocity.Y != 0)
		{
			animatedSprite2D.Animation = "up";
			animatedSprite2D.FlipV = velocity.Y >0;
		}
	}

	private void _on_Body_Entered(Node2D body)
	{
		Hide(); //player disappears after being hit
		EmitSignal(SignalName.Hit);
		// must be deferred as we cant change physics properties on a physics callback
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	[Signal]
	public delegate void HitEventHandler();

	[Export]
	public int Speed { get; set; } = 400; // speed of character in pixels/sec

	public Vector2 ScreenSize; // size of the game window


}
