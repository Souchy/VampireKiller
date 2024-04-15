using Godot;
using Godot.Sharp.Extras;
using System;
using vampirekiller.glaceon.util;
using vampirekiller.logia;
using VampireKiller.eevee.vampirekiller.eevee.spells;


/// <summary>
/// Main client (login, main menu, encyclopedia, shop...)
/// </summary>
public partial class Lapis : Node
{

    public override void _EnterTree()
    {
        base._EnterTree();
        var env = AssetCache.Load<PackedScene>(Paths.scenes + "LapisEnvironment.tscn").Instantiate<Node>();
        this.AddChild(env, true);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
    }

}
