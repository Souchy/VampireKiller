using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using Util.communication.events;
using Util.entity;
using Util.structures;
using vampirekiller.eevee.enums;
using vampirekiller.glaceon.util;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.spells;

public partial class UiSapphire : Control
{
    [NodePath]
    public Label LblFps { get; set; }
    [NodePath]
    public Label LblPlayerPos { get; set; }
    [NodePath]
    public Label LblLastRaycast { get; set; }
    [NodePath]
    public Label LblProjCount { get; set; }
    [NodePath]
    public Label LblCreatureCount { get; set; }
    

    [NodePath]
    public UiSlotActive UiSlotActive1 { get; set; }
    [NodePath]
    public UiSlotActive UiSlotActive2 { get; set; }
    [NodePath]
    public UiSlotActive UiSlotActive3 { get; set; }
    [NodePath]
    public UiSlotActive UiSlotActive4 { get; set; }

    private CreatureNode player { get; set; }
    //private Sapphire sapphire { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        this.player = this.GetParent<PlayerNode>();
        //this.sapphire = (Sapphire) this.FindParent("Sapphire");
        //GD.Print("UiSapphire ready");
        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;
        if(player.creatureInstance != null)
        {
            setPlayer(player.creatureInstance);
        }
        EventBus.centralBus.subscribe(this, nameof(onRaycast));
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;
        EventBus.centralBus.unsubscribe(this, nameof(onRaycast));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        /// FIXMEEEEEEEEEEEEEEEEEEEE MULTIPLAYER NO PLAYER + NO SKILL ICONS
        //if (player == null)
        //{
        //    var players = sapphire.Players.GetChildren<PlayerNode>();
        //    var crea = Universe.fight.creatures.get(c => c.creatureGroup == EntityGroupType.Players);
        //    setPlayer(crea.entityUid);
        //}
        //// Le set player devrait etre fait par RPC, sauf en local.
        //else
        //{
            LblPlayerPos.Text = "player: " + player.GlobalPosition;
        //}
        var fps = Engine.GetFramesPerSecond();
        LblFps.Text = "fps: " + fps.ToString();

        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;
        LblProjCount.Text = "projectiles: " + Universe.fight?.projectiles.size();
        LblCreatureCount.Text = "creatures: " + Universe.fight?.creatures.size();
    }

    /// <summary>
    /// Set le player par RPC, sauf en local
    /// FIXME: si on est Online, le fight sera vide et on aura aucun data sur lequal se baser ni se subscribe
    /// </summary>
    [Rpc]
    public void setPlayer(CreatureInstance crea) //ID creatureId)
    {
        if (Universe.isOnline && !this.Multiplayer.IsServer())
            return;
        //this.player = crea.get<CreatureNode>();

        //var crea = Universe.fight.creatures.get(c => c.entityUid == creatureId);

        // update active skills
        crea.activeSkills.GetEntityBus().subscribe(this, nameof(onSetActive));
        for (int i = 0; i < crea.activeSkills.values.Count(); i++)
        {
            onSetActive(crea.activeSkills, i, crea.activeSkills.getAt(i));
        }
    }


    [Subscribe(nameof(onRaycast))]
    public void onRaycast(CreatureNode? crea, Vector3? pos)
    {
        LblLastRaycast.Text = "raycast: " + pos + " - " + crea?.Name;
    }

    /// <summary>
    /// TODO: vu que le UiGame n'est pas instancie pour chq player cote serveur, 
    /// on doit surement utiliser un RPC pour envoyer les nouveaux actifs.
    /// Mais quand on est en local solo, alors....event? au demarrage du game?
    /// </summary>
    [Subscribe(nameof(SmartList<SpellInstance>.setAt))]
    public void onSetActive(SmartList<SpellInstance> skills, int index, SpellInstance skill)
    {
        var model = skill.getModel();
        var tex = AssetCache.Load<Texture2D>(model.iconPath);

        var slot = getSlot(index);
        slot.BtnActive.Icon = tex;
        // cooldown bar? need to subscribe to something
        //slot.CooldownBar.Value = 1;
    }

    private UiSlotActive getSlot(int index)
    {
        switch(index)
        {
            case 0: return UiSlotActive1;
            case 1: return UiSlotActive2;
            case 2: return UiSlotActive3;
            case 3: return UiSlotActive4;
        }
        return default;
    }

}
