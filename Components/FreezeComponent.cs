using Godot;
using System;

[GlobalClass]
public partial class FreezeComponent : DamagingComponent
{
	[Export]
	private float speedMultiplier = 0.5f;
	[Export]
	private float duration = 3f;
	[Export]
	private PackedScene freezeScene; 

	public override void ApplyDamage(Node3D target)
	{
		HealthComponent h = target.GetNodeOrNull<HealthComponent>("HealthComponent");
		if (h != null) h.TakeDamage(Damage);
		if (freezeScene != null)
		{
			Node3D effect = freezeScene.Instantiate<Node3D>();
			GetTree().CurrentScene.AddChild(effect); 
			effect.GlobalPosition = GlobalPosition; 
		}
		
		MoveComponent m = target.GetNodeOrNull<MoveComponent>("MoveComponent");
		if (m != null)
		{
			float lastSpeed = m.GetSpeed();
			GD.Print($"Враг {target.Name} заморожен. Скорость снижена!");
			m.SetSpeed(lastSpeed *= speedMultiplier);

			if (GodotObject.IsInstanceValid(m))
			{
				GetTree().CreateTimer(duration).Timeout += () => 
				{
					if (GodotObject.IsInstanceValid(target) && GodotObject.IsInstanceValid(m))
                	{
						m.SetSpeed(lastSpeed);
						GD.Print($"Эффект заморозки на {target.Name} закончился. Скорость вернулась.");
					}
				};
			}
		}

		if (IsOneShot) GetParent().QueueFree();
	}
}
