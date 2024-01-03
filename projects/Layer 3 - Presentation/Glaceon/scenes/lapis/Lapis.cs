using Godot;
using Godot.Sharp.Extras;
using System;
using vampirekiller.glaceon.util;
using VampireKiller.eevee.vampirekiller.eevee.spells;


/// <summary>
/// Main client (login, main menu, encyclopedia, shop...)
/// </summary>
public partial class Lapis : Node
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }


}
