
using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using Util.communication.commands;
using Util.communication.events;
using Util.entity;
using vampierkiller.logia;
using vampierkiller.logia.commands;
using vampirekiller.eevee.actions;
using VampireKiller.eevee.vampirekiller.eevee;
using vampirekiller.logia.extensions;
using vampirekiller.logia;

namespace Glaceon;

/// <summary>
/// 
/// </summary>
public partial class Glaceon : Node
{

    [NodePath]
    public Lapis Lapis { get; set; }
    [NodePath]
    public Sapphire game { get; set; }


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
        this.currentScene = this.Lapis;
    }

    [Subscribe(Fight.EventSet)]
    public void onFigthStarted(Fight fight)
    {
        if(fight == null)
        {
            this.changeScene(this.Lapis);
        } else
        {
            if (this.currentScene != this.game)
            {
                this.changeScene(this.game);
            }
        }
    }

    [Subscribe(Events.EventChangeScene)]
    public void onChangeSceneEvent(string sceneName)
    {
        if(sceneName == Events.SceneMain)
        {
            changeScene(Lapis);
        }
        if(sceneName == Events.SceneFight)
        {
            changeScene(game);
        }
    }

    private void changeScene(Node newScene)
    {
        this.RemoveChild(this.currentScene);
        this.AddChild(newScene);
        this.currentScene = newScene;
    }

}
