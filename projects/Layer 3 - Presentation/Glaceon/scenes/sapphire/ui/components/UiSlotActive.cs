using Godot;
using Godot.Sharp.Extras;
using System;
using vampirekiller.glaceon.util;
using VampireKiller.eevee.vampirekiller.eevee.spells;

/// <summary>
/// TODO: also need a tooltip label
/// </summary>
public partial class UiSlotActive : VBoxContainer
{
    [NodePath]
    public ProgressBar CooldownBar { get; set; }
    [NodePath]
    public Button BtnActive { get; set; }

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
