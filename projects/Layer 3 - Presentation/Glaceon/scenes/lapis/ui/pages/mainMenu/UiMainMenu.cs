using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.commands;
using vampierkiller.logia;
using vampierkiller.logia.commands;


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

	[Inject]
	public ICommandPublisher publisher { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.OnReady();
		this.Inject();
		this.BtnPlay.ButtonDown += pressPlay;
	}

	public void pressPlay()
	{
		this.publisher.publish(new CommandPlayGame());
	}

}
