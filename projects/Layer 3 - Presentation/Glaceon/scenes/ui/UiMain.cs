using Godot;
using Godot.Sharp.Extras;
using System;

public partial class UiMain : Control
{
    [NodePath]
    public Button BtnPlay { get; set; }
    [NodePath]
    public Button BtnLobbies { get; set; }
    [NodePath]
    public Button BtnSettings { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
