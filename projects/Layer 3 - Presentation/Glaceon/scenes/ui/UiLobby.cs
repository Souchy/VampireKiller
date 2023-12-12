using Godot;
using Godot.Sharp.Extras;
using System;

public partial class UiLobby : Control
{
    [NodePath]
    public Button BtnHost { get; set; }
    [NodePath]
    public Button BtnJoin { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        BtnHost.ButtonDown += BtnHost_ButtonDown;
        BtnJoin.ButtonDown += BtnJoin_ButtonDown;
	}

    private void BtnHost_ButtonDown()
    {
        // TODO should send command to Application layer
        throw new NotImplementedException();
    }

    private void BtnJoin_ButtonDown()
    {
        // TODO should send command to Application layer
        throw new NotImplementedException();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
