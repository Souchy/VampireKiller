using Godot;
using Godot.Sharp.Extras;
using System;
using vampierkiller.logia;


namespace vampirekiller.glaceon.sapphire.ui.esc_menu.map;

public partial class UiMapNode : Control
{
    [NodePath]
    public TextureRect TextureRect { get; set; }
    [NodePath]
    public Label Label { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        this.Inject();
    }

}
