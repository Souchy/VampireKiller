using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using Util.communication.events;

public partial class UiGame : Control
{
    [NodePath]
    public Label LblFps { get; set; }
    [NodePath]
    public Label LblPlayerPos { get; set; }
    [NodePath]
    public Label LblLastRaycast { get; set; }
    // public UiSlotActive slot { get; set; }

    private CreatureNode player { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        EventBus.centralBus.subscribe(this, nameof(onRaycast));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (player == null)
        {
            var crea = Universe.fight.creatures.get(c => c.creatureGroup == vampirekiller.eevee.enums.EntityGroupType.Players);
            this.player = crea.get<CreatureNode>();
        }
        else
        {
            LblPlayerPos.Text = "player: " + player.GlobalPosition;
        }
        var fps = Engine.GetFramesPerSecond();
        LblFps.Text = "fps: " + fps.ToString();
    }

    [Subscribe(nameof(onRaycast))]
    public void onRaycast(Vector3 pos)
    {
        LblLastRaycast.Text = "raycast: " + pos;
    }

}
