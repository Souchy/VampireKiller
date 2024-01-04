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
using static CreatureNodeAnimationPlayer;

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

    private const double cacheRefreshTime = 1000;
    private double cacheRefreshDelta = 0;

    private float cachedMovementSpeed = 0;
    protected float defaultSpeed { get; } = 5.0f;

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
        recalculateCache();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        creatureInstance?.remove<ProjectileNode>();
        creatureInstance?.remove<PositionGetter>();
    }


    public override void _PhysicsProcess(double delta)
    {   
        if (Universe.isOnline && !this.IsMultiplayerAuthority()) 
            return;

        refreshCache(delta);

        var direction = getNextDirection();
        var speed = cachedMovementSpeed;

        var velocity = this.Velocity;
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        // If no input, slow down 
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
        }
        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y -= gravity * (float)delta;
        }

        // Set velocity
        this.Velocity = velocity;
        MoveAndSlide();

        // Look at
        Vector3 fowardPoint = this.Position + Velocity * 1;
        Vector3 lookAtTarget = new Vector3(fowardPoint.X, 0, fowardPoint.Z);
        betterLookAt(lookAtTarget);

        // Animation idle/walk
        SupportedAnimation animation = this.Velocity.IsZeroApprox() ? SupportedAnimation.Idle : SupportedAnimation.Walk;
        this.CreatureNodeAnimationPlayer.playAnimation(animation);
    }

    /// <summary>
    /// Permet de debounce le calcul de certaines stats a un interval de temps (ex: 1000ms) plutot que chaque frame
    /// </summary>
    private void refreshCache(double delta)
    {
        cacheRefreshDelta += delta;
        if(cacheRefreshDelta > cacheRefreshTime)
        {
            cacheRefreshDelta = 0;
            recalculateCache();
        }
    }
    private void recalculateCache()
    {
        cachedMovementSpeed = (float) (creatureInstance?.getTotalStat<CreatureTotalMovementSpeed>().value ?? defaultSpeed);
    }

    /// <summary>
    /// Get the next frame's direction
    /// </summary>
    /// <returns></returns>
    protected abstract Vector3 getNextDirection();

    /// <summary>
    /// Get the next frame's direction from the navigation agent, or Zero if no current path.
    /// </summary>
    /// <returns></returns>
    protected Vector3 getNextNavigationDirection()
    {
        // If point & click, set velocity
        if (!NavigationAgent3D.IsNavigationFinished())
        {
            var nextPos = NavigationAgent3D.GetNextPathPosition();
            nextPos.Y = 0;
            var direction = GlobalPosition.DirectionTo(nextPos);
            direction.Y = 0;
            return direction.Normalized();
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
        this.CreatureNodeAnimationPlayer.playAnimation(SupportedAnimation.Attack, attackCallback);
    }

    #region Event Handlers
    [Subscribe(DomainEvents.EventDeath)]
    public void onDeath(CreatureInstance crea) {
        this.CreatureNodeAnimationPlayer.playAnimation(SupportedAnimation.Death);
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

    [Subscribe]
    public void onItemListAdd(object list, object item)
    {
        // check all statements 
        //      modify mesh / material / etc si nécessaire
        recalculateCache();
    }
    [Subscribe]
    public void onItemListRemove(object list, object item)
    {
        recalculateCache();
    }
    [Subscribe]
    public void onStatusListAdd(object list, object item)
    {
        recalculateCache();
    }

    [Subscribe(nameof(SmartList<Status>.remove))]
    public void onStatusListRemove(SmartList<Status> list, Status item)
    {
        //  TODO recalculate cache on status change/item change
        recalculateCache();
    }
    [Subscribe(DomainEvents.EventDamage)]
    public void onDamage(int value)
    {
        var popup = AssetCache.Load<PackedScene>("res://scenes/sapphire/ui/components/UiResourcePopup.tscn").Instantiate<UiResourcePopup>();
        popup.value = value;
        this.AddChild(popup);
    }
    #endregion

    private void updateHPBar()
    {
        var life = this.creatureInstance.getTotalStat<CreatureTotalLife>();
        var max = this.creatureInstance.getTotalStat<CreatureTotalLifeMax>();
        double value = ((double) life.value / (double) max.value) * 100;
        // GD.Print("Crea (" + this.Name + ") update hp %: " + value); // + "............" + Healthbar + " vs " + hpbar);
        Healthbar.Value = value;
    }


}
