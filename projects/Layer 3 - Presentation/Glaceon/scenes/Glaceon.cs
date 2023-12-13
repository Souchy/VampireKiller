using Godot;
using Godot.Sharp.Extras;
using System;

namespace Glaceon;

public partial class Glaceon : Node3D
{

	[NodePath]
	public UiMainMenu mainmenu { get; set; }
	[NodePath]
	public Game game { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// command / signal
	public void changeScene(string scene) 
	{

	}

}
