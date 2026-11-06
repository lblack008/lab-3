using Godot;
using System;

public partial class Bullet : Area3D
{
	// параметры пули
	[Export]
	private float Speed = 30f;

	// движение пули
    public override void _PhysicsProcess(double delta)
    {
        Position += -Transform.Basis.Z * Speed * (float)delta;
    }
}
