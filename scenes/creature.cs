using Godot;
using System;

namespace VampireKiller.scenes;

// [Tool]
public partial class Creature : Node3D
{
	public StatsDictionary stats { get; set; } = new();

    [ExportGroup("My asdf")]
    [Export]
    public Creature_res apple { get; set; }

    [ExportGroup("Some Tests")]
    [Export(PropertyHint.Range, "-10,20,")]
    public int TestExpo { get; set; } = 1;

    public Creature() {

	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
