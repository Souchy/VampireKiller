using System;
using Godot;
using Godot.Collections;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using Util.communication.events;
using Util.entity;
using Util.structures;
using vampierkiller.logia.commands;
using vampirekiller.glaceon.util;
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
        inst.GetEntityBus().subscribe(node);
        node.init(inst);
        Entities.AddChild(node);
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        // faut référence à creaInst.entityUid dans CreatureNode;
        var creatures = this.GetTree().GetNodesInGroup("Projectiles");
        CreatureNode node = findChildrenOfNodeMatchingIdentifiable<CreatureNode>(creatures, inst, node => node.creatureInstance);
        node.QueueFree();
    }

    [Subscribe(nameof(SmartSet<ProjectileInstance>.add))]
    public void onAddProjectile(SmartSet<ProjectileInstance> list, ProjectileInstance inst)
    {
        // TODO projetile nodes
        // CreatureInstance inst = inst.getParent
        // CreatureNode creaNode = this.get...
        // ProjectileNode projNode = GD.Load<PackedScene>(inst.model.meshScenePath).Instantiate<ProjectileNode>();
        // projNode.init(inst);
        // Ajoute le proj à la créature? pour on death ça clear au complet
        // creaNode.addChild(projNode)
        ProjectileNode node = AssetCache.Load<PackedScene>(inst.meshScenePath).Instantiate<ProjectileNode>();
        //inst.GetEntityBus().subscribe(node);
        node.init(inst);
        this.Entities.AddChild(node);
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveProjectile(SmartSet<ProjectileInstance> list, ProjectileInstance inst)
    {
        // CreatureNode sourceNode = inst.originator.get<CreatureNode>();
        var projs = this.GetTree().GetNodesInGroup("Projectiles");
        ProjectileNode node = findChildrenOfNodeMatchingIdentifiable<ProjectileNode>(projs, inst, node => node.projectileInstance);
        node.QueueFree();
    }

    private T? findChildrenOfNodeMatchingIdentifiable<T>(Array<Node> children, Identifiable identifiable, Func<T, Identifiable> uidExtractor) where T : Node
    {
        foreach (var child in children)
        {
            if (child is T)
            {
                ID childUid = uidExtractor((T)child).entityUid;
                if (childUid.Equals(identifiable.entityUid))
                {
                    return (T)child;
                }
            }
        }
        return null;
    }

    private void clearNodes()
    {
        // Cleanup old nodes
        this.Entities.QueueFreeChildren();
    }

}
