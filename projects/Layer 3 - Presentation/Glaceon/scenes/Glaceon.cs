using Espeon;
using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee;

namespace Glaceon;

public partial class Glaceon : Node
{

    [NodePath]
    public UiMainMenu mainmenu { get; set; }
    [NodePath]
    public Game game { get; set; }

    private EspeonCommandHandler commandHandler;
    private Node currentScene { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        this.commandHandler = new EspeonCommandHandler();
        this.commandHandler.state.GetEntityBus().subscribe(this);
        this.mainmenu.initCommandHandler(this.commandHandler.handle);

        // Enforce only one current active scene
        this.RemoveChild(this.game);
        this.currentScene = this.mainmenu;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    [Subscribe("fight.started")]
    public void onFigthStarted(Fight fight)
    {
        if (this.currentScene != this.game)
        {
            this.changeScene(this.game);
        }
        
        this.game.startFight(fight);
    }

    private void changeScene(Node newScene)
    {
        this.RemoveChild(this.currentScene);
        this.AddChild(newScene);
        this.currentScene = newScene;
    }

}
