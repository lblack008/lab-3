using Godot;
using System;

public partial class Effect : Node3D
{
    public override void _Ready()
    {
        GetTree().CreateTimer(0.6f).Timeout += () => QueueFree();
    }
}