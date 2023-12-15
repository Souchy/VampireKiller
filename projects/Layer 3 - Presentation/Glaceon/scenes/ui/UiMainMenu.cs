using Espeon.commands;
using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.commands;


/// <summary>
/// 
/// </summary>
public partial class UiMainMenu : Node
{
	[NodePath]
	public Button BtnPlay { get; set; }
	[NodePath]
	public Button BtnLobbies { get; set; }
	[NodePath]
	public Button BtnSettings { get; set; }
	
	
	[NodePath("%Camera3D")]
	public Camera3D camera3D { get; set; }

	private Action<ICommand> commandHandlerHook;
	private bool isActivated = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.OnReady();
		this.BtnPlay.ButtonDown += pressPlay;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void initCommandHandler(Action<ICommand> commandHandlerHook)
	{
		this.commandHandlerHook = commandHandlerHook;
	}

	public void pressPlay()
	{
		// var game = GD.Load<PackedScene>("res://game.tscn").Instantiate<Game>();
		// this.GetTree().Root.AddChild(game);

		//this.GetTree().Root.FindChild("game").
		//this.Hide();
		this.commandHandlerHook(new CommandPlayGame());
	}

}
