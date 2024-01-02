using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using Util.entity;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.stats;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;
using vampirekiller.logia.extensions;
using vampirekiller.glaceon.util;
using vampirekiller.eevee.stats.schemas.resources;
using Logia.vampirekiller.logia;
using vampirekiller.eevee.statements.schemas;
using Util.structures;
using vampirekiller.eevee.util;
using VampireKiller.eevee;

/// <summary>
/// Properties that need to be shown:
/// - life, lifeMax
/// - mana, manaMax MAYBE
/// - position, direction, speed (transform)
/// </summary>
public partial class CreatureNode : CharacterBody3D
{
    public CreatureInstance creatureInstance;

    [NodePath]
    public Node3D MeshInstance3D { get; set; }
    // [NodePath]
    // public Node3D Model3d { get; set; }
    //[NodePath]
    //public AnimationPlayer player { get; set; }

    // [NodePath]
    [NodePath]
    public NavigationAgent3D NavigationAgent3D { get; set; }

    [NodePath("SubViewport/UiResourceBars")]
    public MarginContainer UiResourceBars { get; set; }
    [NodePath("SubViewport/UiResourceBars/VBoxContainer/Healthbar")]
    public ProgressBar Healthbar { get; set; }
    [NodePath]
    public Label3D LabelOwner { get; set; }
    [NodePath]
    public Node3D StatusEffects { get; set; }
    [NodePath]
    public MultiplayerSpawner StatusEffectsSpawner { get; set; }

    public float Speed = 5.0f;
    public float JumpVelocity = 6.0f;

    public override void _Ready()
    {
        this.OnReady();
        // GD.Print(this.Name + " ready");
        if (creatureInstance != null)
        {
            // this.GlobalPosition = creatureInstance.spawnPosition;
            updateHPBar();
            LabelOwner.Text = "" + creatureInstance.playerId;
        }
        StatusEffectsSpawner.AddSpawnableScene("res://scenes/db/spells/fireball/fireball_burn.tscn");
    }

    protected bool physicsNavigationProcess(double delta)
    {
        // If point & click, set velocity
        if (!NavigationAgent3D.IsNavigationFinished())
        {
            var nextPos = NavigationAgent3D.GetNextPathPosition();
            nextPos.Y = 0;
            var direction = GlobalPosition.DirectionTo(nextPos);
            direction.Y = 0;
            Velocity = direction * Speed;
            betterLookAt(nextPos);
            MoveAndSlide();
            return true;
        }
        return false;
    }

    protected void betterLookAt(Vector3 nextPos)
    {
        // check is to avoid following warning: Up vector and direction between node origin and target are aligned, look_at() failed
        if (!Position.IsEqualApprox(nextPos) && !Vector3.Up.Cross(nextPos - this.Position).IsZeroApprox())
        {
            MeshInstance3D.LookAt(nextPos);
            MeshInstance3D.RotateY(Mathf.Pi);
        }
    }


    public void init(CreatureInstance crea)
    {
        // GD.Print(this.Name + " init");
        creatureInstance = crea;
        creatureInstance.GetEntityBus().subscribe(this);
        creatureInstance.statuses.GetEntityBus().subscribe(this);
        creatureInstance.activeSkills.GetEntityBus().subscribe(this);
        creatureInstance.items.GetEntityBus().subscribe(this);

        creatureInstance.set<CreatureNode>(this);
        //creatureInstance.getPositionHook = () => this.GlobalPosition;
        //creatureInstance.setPositionHook = (Vector3 v) => this.GlobalPosition = v;
        //creatureInstance.set<Func<Vector3>>(() => this.GlobalPosition);
        creatureInstance.set<PositionGetter>(() => this.GlobalPosition);
    }

    //TEST TDOTDO ka sdklajm SD
    public override void _ExitTree()
    {
        base._ExitTree();
        creatureInstance?.remove<ProjectileNode>();
        creatureInstance?.remove<PositionGetter>();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        if(this.Name.ToString().Contains("player_"))
        {
            var id = this.Name.ToString().Replace("player_", "");
            this.SetMultiplayerAuthority(int.Parse(id));
        }
        // GD.Print(this.Name + " enter tree");
        if (creatureInstance != null)
        {
            this.GlobalPosition = creatureInstance.spawnPosition;
        }
    }


    [Subscribe(CreatureInstance.EventUpdateStats)]
    public void onStatChanged(CreatureInstance crea, IStat stat)
    {
        // GD.Print("CreatureNode: onStatChanged: " + stat.GetType().Name + " = " + stat.genericValue);
        // todo regrouper les life stats en une liste<type> automatique genre / avoir une annotation [Life] p.ex, etc
        if (stat is CreatureAddedLife || stat is CreatureAddedLifeMax || stat is CreatureBaseLife || stat is CreatureBaseLifeMax || stat is CreatureIncreasedLife || stat is CreatureIncreasedLifeMax)
        {
            updateHPBar();
        }
    }

    // [Subscribe]
    // public void onItemListAdd(object list, object item)
    // {
    //     // check all statements 
    //     //      modify mesh / material / etc si nécessaire
    // }
    // [Subscribe]
    // public void onItemListRemove(object list, object item)
    // {

    // }
    // [Subscribe]
    // public void onStatusListAdd(object list, object item)
    // {

    // }

    [Subscribe(nameof(SmartList<Status>.remove))]
    public void onStatusListRemove(SmartList<Status> list, Status item)
    {

    }

    private void updateHPBar()
    {
        var life = this.creatureInstance.getTotalStat<CreatureTotalLife>();
        var max = this.creatureInstance.getTotalStat<CreatureTotalLifeMax>();
        double value = ((double) life.value / (double) max.value) * 100;
        // GD.Print("Crea (" + this.Name + ") update hp %: " + value); // + "............" + Healthbar + " vs " + hpbar);
        Healthbar.Value = value;
    }

    [Subscribe("damage")]
    public void onDamage(int value)
    {
        var popup = AssetCache.Load<PackedScene>("res://scenes/sapphire/ui/components/UiResourcePopup.tscn").Instantiate<UiResourcePopup>();
        popup.value = value;
        this.AddChild(popup);
    }

}
