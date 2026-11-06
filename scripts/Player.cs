using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export]
    private float _speed = 8f;
    [Export]
    private float _gravity = 20f;
    [Export]
	private PackedScene BulletScene;
    [Export]
    private Camera3D _camera;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 input = Input.GetVector("left", "right", "up", "down");

        Vector3 direction = new Vector3(input.X, 0, input.Y);

        Velocity = new Vector3(
            direction.X * _speed,
            Velocity.Y,
            direction.Z * _speed);

        if (!IsOnFloor())
        {
            Velocity += Vector3.Down * _gravity * (float)delta;
        }
        
        MoveAndSlide();
        // LookAtMouse();
    }
    public override void _Input(InputEvent @event)
    {
        //выстрел по ЛКМ
        if (@event.IsActionPressed("shoot"))
        {
           Shoot();
        }
        //выход игры по Escape
        if (@event.IsActionPressed("exit"))
        {
            GetTree().Quit();
        }
    }

    public override void _Process(double delta)
    {
        LookAtMouse();
    }

    private void LookAtMouse()
    {
        Vector2 screenCenter = (Vector2)GetViewport().GetWindow().Size / 2f;
        Vector2 mousePos = GetViewport().GetMousePosition();
        float angle = screenCenter.AngleToPoint(mousePos);
        Rotation = new Vector3(0, -angle, 0);
    }

    private void Shoot()
	{
		if (BulletScene != null)
		{
			Bullet b = BulletScene.Instantiate<Bullet>();
			GetTree().CurrentScene.AddChild(b);
			b.GlobalPosition = this.GlobalPosition;
			b.GlobalRotation = this.GlobalRotation;
		}
    }
}