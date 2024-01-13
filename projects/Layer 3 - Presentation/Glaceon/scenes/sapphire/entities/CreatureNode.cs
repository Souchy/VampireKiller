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
using vampirekiller.umbreon.commands;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.creature;
using scenes.sapphire.entities.component;
using static System.Collections.Specialized.BitVector32;
using vampirekiller.logia;

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
    public CharacterModelNode Model { get; set; }
    public CreatureNodeAnimationPlayer CreatureNodeAnimationPlayer { get => Model.AnimationPlayer as CreatureNodeAnimationPlayer; }

    [NodePath]
    public NavigationAgent3D NavigationAgent3D { get; set; }

    [NodePath]
    public Sprite3D ResourceBars { get; set; }

    [NodePath("SubViewport/UiResourceBars")]
    public MarginContainer UiResourceBars { get; set; }
    [NodePath("SubViewport/UiResourceBars/VBoxContainer/HealthBar")]
    public ProgressBar Healthbar { get; set; }
    //[NodePath("SubViewport/UiResourceBars/VBoxContainer/ShieldBar")]
    //public ProgressBar Shieldbar { get; set; }
    [NodePath]
    public Label3D LabelOwner { get; set; }
    [NodePath]
    public Node3D StatusEffects { get; set; }
    [NodePath]
    public MultiplayerSynchronizer MultiplayerSynchronizer { get; set; }
    [NodePath]
    public MultiplayerSpawner StatusEffectsSpawner { get; set; }
    [NodePath]
    public MultiplayerSpawner ModelSpawner { get; set; }

    /// <summary>
    /// In seconds
    /// </summary>
    private const double cacheRefreshTime = 1;
    private double cacheRefreshDelta = 0;

    protected float defaultSpeed { get; } = 5.0f;
    private float cachedTotalMovementSpeed = 0;
    private float cachedIncreasedMovementSpeed;

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
            updateHPBar();
            LabelOwner.Text = "" + creatureInstance.playerId;

            // Load creature skin & skill skins
            setSkin(creatureInstance.currentSkin);
        }

        if (Universe.isOnline && !this.Multiplayer.IsServer())
            return;
        ModelSpawner._SpawnableScenes = AssetCache.models.ToArray();
        StatusEffectsSpawner._SpawnableScenes = AssetCache.skills.ToArray();
        //StatusEffectsSpawner.AddSpawnableScene("res://vampireassets/spells/fireball/fireball_burn.tscn");
    }

    public void init(CreatureInstance crea)
    {
        // GD.Print(this.Name + " init");
        creatureInstance = crea;
        // Subscribe to all events
        creatureInstance.GetEntityBus().subscribe(this);
        creatureInstance.statuses.GetEntityBus().subscribe(this);
        creatureInstance.activeSkills.GetEntityBus().subscribe(this);
        creatureInstance.items.GetEntityBus().subscribe(this);
        // Set components
        creatureInstance.set<CreatureNode>(this);
        creatureInstance.set<PositionGetter>(() => this.GlobalPosition);
        // Cache stats
        recalculateCache();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        creatureInstance?.remove<ProjectileNode>();
        creatureInstance?.remove<PositionGetter>();
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
        cachedTotalMovementSpeed = (float) (creatureInstance?.getTotalStat<CreatureTotalMovementSpeed>().value ?? defaultSpeed);
        cachedIncreasedMovementSpeed = (float) (creatureInstance?.getTotalStat<CreatureIncreasedMovementSpeed>().value ?? 0);
    }

    public void setSkin(CreatureSkin skin)
    {
        var oldModel = Model;
        var scriptPath = Paths.entities + "component/" + nameof(CharacterModelNode) + ".cs";
        var scene = AssetCache.Load<PackedScene>(skin.scenePath, ".tscn", ".glb", ".gltf").Instantiate<Node3D>();
        CharacterModelNode newModel = scene.SafelySetScript<CharacterModelNode>(scriptPath);
        newModel.Name = "Model";

        // Replace model node
        this.RemoveChild(oldModel);
        Model = newModel;
        this.AddChild(Model, true);
        oldModel.QueueFree();

        // Set skin for animations
        this.CreatureNodeAnimationPlayer.skin = creatureInstance.currentSkin;
        // Load animation libraries in the new model's animation player
        foreach (var lib in skin.animationLibraries)
        {
            this.CreatureNodeAnimationPlayer.loadLibrary(lib);
        }
        foreach (var skill in creatureInstance.activeSkills.values)
        {
            onActiveSkillAdd(creatureInstance.activeSkills, skill);
        }
    }

    private void updateHPBar()
    {
        var life = this.creatureInstance.getTotalStat<CreatureTotalLife>();
        var max = this.creatureInstance.getTotalStat<CreatureTotalLifeMax>();
        double value = ((double) life.value / (double) max.value) * 100;
        // GD.Print("Crea (" + this.Name + ") update hp %: " + value); // + "............" + Healthbar + " vs " + hpbar);
        Healthbar.Value = value;
        if(value != 100 && !ResourceBars.Visible)
            ResourceBars.Visible = true;

        // shield bar?
    }


}
