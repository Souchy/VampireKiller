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
using vampirekiller.glaceon.util;
using vampirekiller.logia.extensions;
using VampireKiller.eevee;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee;

public partial class Game : Node
{

    [NodePath]
    public Node Environment { get; set; }
    [NodePath]
    public Node Entities { get; set; }

    [NodePath]
    public Camera3D Camera3D { get; set; }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        EventBus.centralBus.subscribe(this);
    }
    

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var action = new ActionProcessTick(delta);
        Universe.fight.procTriggers(action);
    }


    [Subscribe(Fight.EventSet)]
    public void onChangeFight(Fight fight)
    {
        clearNodes();
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

    [Subscribe(nameof(SmartSet<CreatureInstance>.add))]
    public void onAddCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        CreatureNode node = AssetCache.Load<PackedScene>(inst.model.meshScenePath).Instantiate<CreatureNode>();
        node.init(inst);
        Entities.AddChild(node);
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
        this.Entities.AddChild(node);
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
    }

}
