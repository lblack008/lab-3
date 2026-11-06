using Godot;
using System;

[GlobalClass]
public partial class DamagingComponent : Area3D
{
	[Export] 
	public float Damage {get; set;} = 10f;
	[Export] 
	public string TargetGroup {get; set;} = "";
	[Export] 
	protected bool IsOneShot {get; set;} = true;

	public override void _Ready()
	{
		// Подключаем встроенный сигнал Area3D к методу
		this.BodyEntered += OnBodyEntered;
	}

	protected virtual void OnBodyEntered(Node3D body)
	{
		if (body.IsInGroup(TargetGroup)) ApplyDamage(body);
	}

	public virtual void ApplyDamage(Node3D target)
	{
		HealthComponent h = target.GetNodeOrNull<HealthComponent>("HealthComponent");
		
		if (h != null)
		{
			h.TakeDamage(Damage);
			if (IsOneShot)GetParent().QueueFree(); 
		}
	}
}
