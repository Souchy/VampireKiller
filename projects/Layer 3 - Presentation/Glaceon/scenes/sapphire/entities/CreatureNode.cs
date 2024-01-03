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
using vampirekiller.eevee.spells;

/// <summary>
/// Properties that need to be shown:
/// - life, lifeMax
/// - mana, manaMax MAYBE
/// - position, direction, speed (transform)
/// </summary>
public abstract partial class CreatureNode : CharacterBody3D
{
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    
    public CreatureInstance creatureInstance;

    [NodePath]
    public Node3D Model { get; set; }
    [NodePath]
    public CreatureNodeAnimationPlayer CreatureNodeAnimationPlayer { get; set; }

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

    public override void _ExitTree()
    {
        base._ExitTree();
        creatureInstance?.remove<ProjectileNode>();
        creatureInstance?.remove<PositionGetter>();
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        if (Universe.isOnline && !this.IsMultiplayerAuthority()) 
            return;

        var direction = getNextDirection();

        var velocity = this.Velocity;
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        // If no input, slow down 
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }
        // Add the gravity.
        if (!IsOnFloor())
        {
            // Vector3 velocity = this.Velocity;
            velocity.Y -= gravity * (float)delta;
            this.Velocity = velocity;
        }
        this.Velocity = velocity;

        Vector3 fowardPoint = this.Position + Velocity * 1;
        Vector3 lookAtTarget = new Vector3(fowardPoint.X, 0, fowardPoint.Z);
        betterLookAt(lookAtTarget);
        MoveAndSlide();

        if (this.Velocity.IsZeroApprox())
        {
            this.CreatureNodeAnimationPlayer.playAnimation(CreatureNodeAnimationPlayer.SupportedAnimation.Idle);
        } else
        {
            this.CreatureNodeAnimationPlayer.playAnimation(CreatureNodeAnimationPlayer.SupportedAnimation.Walk);
        }
    }

    protected abstract Vector3 getNextDirection();

    protected Vector3 getNextNavigationDirection()
    {
        // If point & click, set velocity
        if (!NavigationAgent3D.IsNavigationFinished())
        {
            var nextPos = NavigationAgent3D.GetNextPathPosition();
            nextPos.Y = 0;
            var direction = GlobalPosition.DirectionTo(nextPos);
            direction.Y = 0;
            return direction;
        }
        return Vector3.Zero;
    }

    protected void betterLookAt(Vector3 nextPos)
    {
        // check is to avoid following warning: Up vector and direction between node origin and target are aligned, look_at() failed
        if (!Position.IsEqualApprox(nextPos) && !Vector3.Up.Cross(nextPos - this.Position).IsZeroApprox())
        {
            Model.LookAt(nextPos);
            Model.RotateY(Mathf.Pi);
        }
    }
    
    protected void playAttack(Action attackCallback)
    {
        this.CreatureNodeAnimationPlayer.playAnimation(CreatureNodeAnimationPlayer.SupportedAnimation.Attack, attackCallback);
    }

    // TODO regroupe les action de cast. 
    // protected void inpuCastSkill(int slot, raycast) {
	// 		if (raycast == Vector3.Zero)
	// 			raycast = getRayCast();
	// 		var cmd = new CommandCast(this.creatureInstance, raycast, 0); //-this.Transform.Basis.Z, 1);
	// 		this.attack(() => this.publisher.publish(cmd));
    // }

    [Subscribe(DomainEvents.EventDeath)]
    public void onDeath(CreatureInstance crea) {
        this.CreatureNodeAnimationPlayer.playAnimation(CreatureNodeAnimationPlayer.SupportedAnimation.Death);
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
