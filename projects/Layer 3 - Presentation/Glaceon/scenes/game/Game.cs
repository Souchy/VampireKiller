using Glaceon;
using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using Util.entity;
using Util.structures;
using VampireKiller.eevee;
using VampireKiller.eevee.creature;

public partial class Game : Node3D
{
    [NodePath]
    public Node Players { get; set; }

    [NodePath]
    public Node Creatures { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Main.fight.creatures.GetEntityBus().subscribe(this);
        Main.fight.projectiles.GetEntityBus().subscribe(this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}


    [Subscribe(nameof(SmartSet<CreatureInstance>.add))]
    public void onAddCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        CreatureNode node = GD.Load<PackedScene>(inst.model.meshScenePath).Instantiate<CreatureNode>();
        inst.GetEntityBus().subscribe(node);
        node.init(inst);
        Creatures.AddChild(node);
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {

    }

    [Subscribe(nameof(SmartSet<Projectile>.add))]
    public void onAddProjectile(SmartSet<Projectile> list, Projectile inst)
    {
        // TODO projetile nodes
        // CreatureInstance inst = inst.getParent
        // CreatureNode node = this.get...
        // ProjectileNode node = GD.Load<PackedScene>(inst.model.meshScenePath).Instantiate<ProjectileNode>();
        // node.init(inst);
        // node.addChild(node)
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveProjectile(SmartSet<Projectile> list, Projectile inst)
    {

    }

}
