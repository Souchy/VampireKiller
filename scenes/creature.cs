using Godot;
using System;

namespace VampireKiller.scenes;

public partial class Creature : Node3D
{
    public StatsDictionary stats { get; set; } = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
