using System;
using System.Linq;
using Godot;
using Godot.Collections;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using Util.communication.events;
using Util.ecs;
using Util.entity;
using Util.structures;
using vampierkiller.logia.commands;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.statements.schemas;
using vampirekiller.eevee.util;
using vampirekiller.glaceon.util;
using vampirekiller.logia.extensions;
using VampireKiller.eevee;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee;

/// <summary>
/// Game client (fight)
/// </summary>
public partial class Sapphire : Node
{

    [NodePath]
    public Node Environment { get; set; }
    [NodePath]
    public Node Entities { get; set; }
    [NodePath]
    public Node Projectiles { get; set; }
    [NodePath]
    public Node Players { get; set; }
    [NodePath]
    public Node Effects { get; set; }

    [NodePath] public MultiplayerSpawner EntitySpawner { get; set; }
    [NodePath] public MultiplayerSpawner PlayerSpawner { get; set; }
    [NodePath] public MultiplayerSpawner EffectSpawner { get; set; }
    [NodePath] public MultiplayerSpawner ProjectileSpawner { get; set; }

    public Sapphire()
    {
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        GD.Print("Sapphire ready");
        clearNodes();

        EntitySpawner.AddSpawnableScene("res://scenes/sapphire/entities/CreatureNode.tscn");
        EntitySpawner.AddSpawnableScene("res://scenes/sapphire/entities/EnemyNode.tscn");
        EntitySpawner.AddSpawnableScene("res://scenes/db/creatures/Orc.tscn");
        PlayerSpawner.AddSpawnableScene("res://scenes/sapphire/entities/PlayerNode.tscn");
        EffectSpawner.AddSpawnableScene("res://scenes/sapphire/entities/effects/FxNode.tscn");
        EffectSpawner.AddSpawnableScene("res://scenes/db/spells/shockNova.tscn");
        EffectSpawner.AddSpawnableScene("res://scenes/db/spells/fireball/fireball_explosion.tscn");
        EffectSpawner.AddSpawnableScene("res://scenes/db/spells/fireball/fireball_burn.tscn");
        ProjectileSpawner.AddSpawnableScene("res://scenes/sapphire/entities/effects/ProjectileNode.tscn");
        ProjectileSpawner.AddSpawnableScene("res://scenes/db/spells/fireball/fireball_projectile.tscn");
        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;
        EventBus.centralBus.subscribe(this);
    }
    

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;
        //var action = new ActionProcessTick(delta);
        //Universe.fight.procTriggers(action);
    }


    [Subscribe(Fight.EventSet)]
    public void onSetFight(Fight fight)
    {
        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;
        if (fight == null)
            return;
        fight.creatures.GetEntityBus().subscribe(this);
        fight.projectiles.GetEntityBus().subscribe(this);

        // Instantiate already existing creatures
        foreach (var creature in fight.creatures.values)
        {
            this.onAddCreatureInstance(fight.creatures, creature);
        }
    }

    [Subscribe(nameof(Dispose))]
    public void onFightDispose(Fight fight)
    {
        clearNodes();
    }


    [Subscribe(SpawnFxSchema.EventFx)]
    public void onFxScene(string scene, Vector3? spawnPos, Entity? attachEntity, bool follow, Status? status)
    {
        if(spawnPos != null)
        {
            var node = AssetCache.Load<PackedScene>(scene).Instantiate<FxNode>();
            node.init(status);
            Effects.AddChild(node, true);
            node.GlobalPosition = (Vector3) spawnPos!;
        }
        else
        if(attachEntity != null)
        {
            if (follow && attachEntity.get<CreatureNode>() != null)
            {
                var creaNode = attachEntity.get<CreatureNode>();
                var node = AssetCache.Load<PackedScene>(scene).Instantiate<FxNode>();
                node.init(status);
                creaNode.StatusEffects.AddChild(node, true);
            }
            // normalement, si on fait ca, le spawnPos serait surement set, c'est le meme usecase
            else
            {
                var getter = attachEntity.get<PositionGetter>();
                if(getter == null)
                    return;
                var pos = getter();
                var node = AssetCache.Load<PackedScene>(scene).Instantiate<FxNode>();
                node.init(status);
                Effects.AddChild(node, true);
                node.GlobalPosition = pos;
            }
        }
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.add))]
    public void onAddCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        CreatureNode node = AssetCache.Load<PackedScene>(inst.model.meshScenePath).Instantiate<CreatureNode>();
        node.init(inst);
        if(inst.creatureGroup == EntityGroupType.Players)
        {
            node.Name = "player_" + inst.playerId;
            Players.AddChild(node, true);
        }
        else
        if(inst.creatureGroup == EntityGroupType.Enemies)
        {
            Entities.AddChild(node, true);
        }
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        CreatureNode node = inst.get<CreatureNode>();
        // GD.Print("Game on remove creature: " + node);
        node.QueueFree();
    }

    [Subscribe(nameof(SmartSet<ProjectileInstance>.add))]
    public void onAddProjectile(SmartSet<ProjectileInstance> list, ProjectileInstance inst)
    {
        ProjectileNode node = AssetCache.Load<PackedScene>(inst.meshScenePath).Instantiate<ProjectileNode>();
        node.init(inst);
        //this.Entities.AddChild(node);
        this.Projectiles.AddChild(node, true);
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveProjectile(SmartSet<ProjectileInstance> list, ProjectileInstance inst)
    {
        ProjectileNode node = inst.get<ProjectileNode>();
        // GD.Print("Game on remove proj: " + node);
        node.QueueFree();
    }

    private void clearNodes()
    {
        // Cleanup old nodes
        this.Entities.QueueFreeChildren();
        this.Projectiles.QueueFreeChildren();
        this.Players.QueueFreeChildren();
        this.Effects.QueueFreeChildren();
    }

}
