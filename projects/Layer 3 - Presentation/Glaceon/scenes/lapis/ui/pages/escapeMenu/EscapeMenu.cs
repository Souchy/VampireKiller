using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using vampirekiller.logia;

public partial class EscapeMenu : Control
{

	[NodePath] public Button BtnResume { get; set; }
	[NodePath] public Button BtnOptions { get; set; }
	[NodePath] public Button BtnQuitToMain { get; set; }
	[NodePath] public Button BtnQuit { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        BtnResume.ButtonUp += clickResume;
        BtnOptions.ButtonUp += clickOptions;
        BtnQuitToMain.ButtonUp += clickQuitToMain;
        BtnQuit.ButtonUp += clickQuit;
	}

	private void clickResume() 
    {
        this.Hide();
	}

    private void clickOptions()
    {
        EventBus.centralBus.publish(Events.EventChangeScene, Events.SceneSettings);
    }

    private void clickQuitToMain()
    {
        EventBus.centralBus.publish(Events.EventChangeScene, Events.SceneMain);
    }

    private void clickQuit()
    {
        GetTree().Quit();
    }

}
