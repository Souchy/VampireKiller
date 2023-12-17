using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using Util.communication.commands;
using Util.communication.events;
using Util.entity;
using vampierkiller.logia;
using vampierkiller.logia.commands;
using VampireKiller.eevee.vampirekiller.eevee;

namespace Glaceon;

public partial class Glaceon : Node
{

    [NodePath]
    public UiMainMenu mainMenu { get; set; }
    [NodePath]
    public Game game { get; set; }

    //private EspeonCommandHandler commandHandler;
    private Node currentScene { get; set; }

    [Inject]
    public ICommandManager commandManager { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        this.Inject();
        EventBus.centralBus.subscribe(this);

        // Enforce only one current active scene
        this.RemoveChild(this.game);
        this.currentScene = this.mainMenu;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    [Subscribe(Fight.EventSet)]
    public void onFigthStarted(Fight fight)
    {
        if(fight == null)
        {
            this.changeScene(this.mainMenu);
        } else
        {
            if (this.currentScene != this.game)
            {
                this.changeScene(this.game);
            }
        }
    }

    private void changeScene(Node newScene)
    {
        this.RemoveChild(this.currentScene);
        this.AddChild(newScene);
        this.currentScene = newScene;
    }

}
