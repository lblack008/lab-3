using Godot;
using System;

[GlobalClass]
public partial class ExplosionComponent : DamagingComponent
{
	// радиус взрыва
	[Export]
	private float radius = 5f;
	// сцена самого выстрела
	[Export]
	private PackedScene explosionScene; 

	// переопределение метода нанесения урона
	public override void ApplyDamage(Node3D target)
	{
		CollisionShape3D collision = GetNodeOrNull<CollisionShape3D>("CollisionShape3D");
		if (collision.Shape is CapsuleShape3D caps)
		{
			caps = (CapsuleShape3D)caps.Duplicate();
			collision.Shape = caps;
			caps.Radius = radius;
		}

		if (explosionScene != null)
		{
			Node3D effect = explosionScene.Instantiate<Node3D>();
			GetTree().CurrentScene.AddChild(effect); 
			effect.GlobalPosition = GlobalPosition; 
		}

		GetTree().CreateTimer(0.05f).Timeout += () => TryShoot();
	
	}

	// попытка выстрела, если это возможно
	private void TryShoot()
	{
		Godot.Collections.Array<Node3D> bodies = GetOverlappingBodies();
		for (int i = 0; i < bodies.Count; i++)
		{
			if (bodies[i] == GetParent()) continue;
			if (bodies[i] is Node3D && bodies[i].IsInGroup(TargetGroup))
			{
				GD.Print($"Взрыв задел врага: {bodies[i].Name}! Нанесено урона: {Damage}");
				var h = bodies[i].GetNodeOrNull<HealthComponent>("HealthComponent");
				h.TakeDamage(Damage);
			}
		}
		GetParent().QueueFree();
	}
}
