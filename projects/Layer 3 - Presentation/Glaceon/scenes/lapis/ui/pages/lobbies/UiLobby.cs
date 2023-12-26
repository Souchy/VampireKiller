using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.commands;
using vampierkiller.logia;
using vampirekiller.logia.commands;

public partial class UiLobby : Control
{
    [NodePath]
    public Button BtnHost { get; set; }
    [NodePath]
    public Button BtnJoin { get; set; }

    [Inject]
    public ICommandPublisher publisher { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.OnReady();
        this.Inject();
        BtnHost.ButtonDown += BtnHost_ButtonDown;
        BtnJoin.ButtonDown += BtnJoin_ButtonDown;
	}

    private void BtnHost_ButtonDown()
    {
        // TODO should send command to Application layer
        publisher.publish(new CommandHost());
        this.QueueFree();
    }

    private void BtnJoin_ButtonDown()
    {
        // TODO should send command to Application layer
        publisher.publish(new CommandJoin());
        this.QueueFree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
