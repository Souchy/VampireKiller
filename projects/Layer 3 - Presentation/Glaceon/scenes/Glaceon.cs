
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
using vampirekiller.glaceon.util;
using vampirekiller.umbreon;
using vampirekiller.logia.net;
using vampirekiller.espeon;

namespace Glaceon;

/// <summary>
/// 
/// </summary>
public partial class Glaceon : Node
{

    [NodePath]
    public Lapis Lapis { get; set; }
    //[NodePath]
    private Sapphire Sapphire { get; set; }

    public Node net { get; set; }

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
        //this.RemoveChild(this.Sapphire);
        this.currentScene = this.Lapis;
    }

    [Subscribe(Events.EventNet)]
    public void onNetEvent(Node node)
    {
        //CallDeferred(nameof(setNet), node);
        this.net?.QueueFree();
        this.net = node; // == "espeon" ? new EspeonNet() : new UmbreonNet();
        this.AddChild(this.net, true);
        this.Multiplayer.MultiplayerPeer = this.net.Multiplayer.MultiplayerPeer;
        GD.Print("Glaceon add net: " + this.net);
        this.net._Ready();
    }


    [Subscribe(Events.EventChangeScene)]
    public void onChangeSceneEvent(string sceneName)
    {
        GetNode("UiLobby")?.QueueFree();
        if(sceneName == Events.SceneMain)
        {
            changeScene(Lapis);
        }
        if(sceneName == Events.SceneFight)
        {
            this.Sapphire?.QueueFree();
            this.Sapphire = AssetCache.Load<PackedScene>("res://scenes/sapphire/Sapphire.tscn").Instantiate<Sapphire>();
            this.changeScene(this.Sapphire);
            this.Sapphire.onSetFight(Universe.fight);
        }
    }

    //[Subscribe(Fight.EventSet)]
    //public void onFigthStarted(Fight fight)
    //{
    //    if(fight == null)
    //    {
    //        this.changeScene(this.Lapis);
    //    } 
    //    else
    //    {
    //        this.Sapphire = AssetCache.Load<PackedScene>("res://scenes/sapphire/Sapphire.tscn").Instantiate<Sapphire>();
    //        this.changeScene(this.Sapphire);
    //        this.Sapphire.onSetFight(fight);
    //    }
    //    //else if (this.currentScene != this.Sapphire)
    //    //{
    //    //    this.changeScene(this.Sapphire);
    //    //}
    //}

    private void changeScene(Node newScene)
    {
        this.RemoveChild(this.currentScene);
        this.AddChild(newScene, true);
        this.currentScene = newScene;
    }

}
