using Godot;
using System;

[GlobalClass]
public partial class ShootComponent : Node3D
{
	// параметры пули
	[Export]
	private PackedScene bulletScene;
	// переменная для расчёта перезарядки выстрелов
	[Export]
	private float fireRate = 2f;

	private Node3D target;
	// таймер для количества выстрелов в секунду
	private Timer shootTimer = new Timer();

	// в начале игры задаётся время перезарядки выстрелов
    public override void _Ready()
    {
        shootTimer.WaitTime = 1.0f / fireRate;
		shootTimer.OneShot = false;
		AddChild(shootTimer);

		shootTimer.Timeout += () => TryShoot();
    }

	// попытка выстрела
	private void TryShoot()
	{
		if (bulletScene != null) SpawnBullet();
		else GD.Print("No bullet scene attached!");
	}

	// создание пули на сцене при выстреле
	private void SpawnBullet()
	{
		Area3D bullet = bulletScene.Instantiate<Area3D>();
		GetTree().CurrentScene.AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition;
		bullet.GlobalRotation = GlobalRotation;
	}

	// включение пушки
	private void TurnOnCannon()
	{
		if (shootTimer.IsStopped()) shootTimer.Start();
	}

	// выключение пушки
	private void TurnOffCannon()
	{
		if (!shootTimer.IsStopped()) shootTimer.Stop();
	}

	// сеттер для цели
	public void SetTarget(Node3D target)
	{
		this.target = target;
	}

	// если цель есть - выстрел, иначе - пушка просто крутится 
    public override void _Process(double delta)
    {
		// проверка, существует ли цель
        if (GodotObject.IsInstanceValid(target))
		{
			// GD.Print("I caught an enemy!");
			Vector3 TargetPos = target.GlobalPosition;
			LookAt(TargetPos);
			TurnOnCannon();
		}
		else
		{
			// если после удаления цели она всё ещё не нулевая - она зануляется искусственно
			if (target != null) target = null;

			// выключение пушки
			TurnOffCannon();
			Rotation = new Vector3(
				Rotation.X,
				Rotation.Y + (float)delta * 0.5f,
				Rotation.Z
			);
		}
    }
}
