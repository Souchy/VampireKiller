using Godot;
using Godot.Sharp.Extras;
using System;


/// <summary>
/// 
/// </summary>
public partial class UiMainMenu : Node3D
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
        BtnPlay.ButtonDown += pressPlay;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


    public void pressPlay()
    {
        // var game = GD.Load<PackedScene>("res://game.tscn").Instantiate<Game>();
        // this.GetTree().Root.AddChild(game);

        //this.GetTree().Root.FindChild("game").
        this.Hide();
    }

}
