using Godot;
using System;

[GlobalClass]
public partial class MoveComponent : Node
{
	// параметры для движения
	[Export]
	protected float speed = 0f;
	// расстояние до цели
	[Export]
	protected float stopRadius = 3f;
	// цель
	protected Node3D target;
	// родительский узел
	protected CharacterBody3D parentNode;

	// в начале игры проверка, что родительский узел - character3d
    public override void _Ready()
    {
        if (GetParent() is CharacterBody3D p) parentNode = p;
    }

	// сеттер для цели
	public void SetTarget(Node3D target)
	{
		this.target = target;
	}

	// геттер для скорости
	public float GetSpeed()
	{
		return speed;
	}

	// изменение скорости
	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}

	// если родительский узел существует - он двигается
    public override void _PhysicsProcess(double delta)
    {
        if (parentNode != null)
		{
			Move(parentNode);
		}
    }

	// движение
	private void Move(CharacterBody3D p)
	{
		// задаётся позиция цели (если её нет, движение в (0, 0, 0))
		Vector3 targetDistance;
		Vector3 position = Vector3.Zero;

		if (target != null) 
		{
			position = target.GlobalPosition;
		}
		targetDistance = position - p.GlobalPosition;
		Vector3 direction = targetDistance.Normalized();
		
		Vector3 velocity = Vector3.Zero;
		if (targetDistance.Length() > stopRadius)
		{
			velocity = new Vector3(direction.X * speed, 0, direction.Z * speed);
		}
		// движение к цели
		p.Velocity = velocity;
		p.MoveAndSlide();
		// GD.Print("My target is " + position + " ");
	}
}
