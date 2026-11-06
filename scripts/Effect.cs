using Godot;
using System;

public partial class Effect : Node3D
{
    // эффект удаляется через некоторое время после создания
    public override void _Ready()
    {
        GetTree().CreateTimer(0.6f).Timeout += () => QueueFree();
    }
}