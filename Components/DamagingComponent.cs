using Godot;
using System;

[GlobalClass]
public partial class DamagingComponent : Area3D
{
	// урон
	[Export] 
	public float Damage {get; set;} = 10f;
	// группа цели для нанесения урона
	[Export] 
	public string TargetGroup {get; set;} = "";
	// одноразовый эффект или нет
	[Export] 
	protected bool IsOneShot {get; set;} = true;

	public override void _Ready()
	{
		// Подключение сигнала
		this.BodyEntered += OnBodyEntered;
	}

	protected virtual void OnBodyEntered(Node3D body)
	{
		// нанесение урона, если объект в соотвествующей группе
		if (body.IsInGroup(TargetGroup)) {
			ApplyDamage(body);
		}
	}

	// реализация нанесения урона
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
