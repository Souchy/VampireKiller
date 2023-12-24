using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using Util.communication.events;
using vampirekiller.glaceon.util;
using VampireKiller.eevee.vampirekiller.eevee.spells;

public partial class UiGame : Control
{
    [NodePath]
    public Label LblFps { get; set; }
    [NodePath]
    public Label LblPlayerPos { get; set; }
    [NodePath]
    public Label LblLastRaycast { get; set; }

    [NodePath]
    public UiSlotActive UiSlotActive1 { get; set; }
    [NodePath]
    public UiSlotActive UiSlotActive2 { get; set; }
    [NodePath]
    public UiSlotActive UiSlotActive3 { get; set; }
    [NodePath]
    public UiSlotActive UiSlotActive4 { get; set; }

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

    [Subscribe]
    public void onSetActive(int index, SpellInstance skill)
    {
        var model = skill.getModel();
        var tex = AssetCache.Load<Texture2D>(model.iconPath);
        UiSlotActive1.BtnActive.Icon = tex;
        // cooldown bar? need to subscribe to something
        //UiSlotActive1.CooldownBar.Value = 1;
    }

}
