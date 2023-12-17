using Godot;
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
        // if (node is EnemyNode)
        // {
        //     EnemyNode enemyNode = (EnemyNode) node;
        //     enemyNode.setTrackingTarget((Node3D) this.Players.GetChild(0));
        // }
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        // faut référence à creaInst.entityUid dans CreatureNode
    }

    [Subscribe(nameof(SmartSet<Projectile>.add))]
    public void onAddProjectile(SmartSet<Projectile> list, Projectile inst)
    {
        // TODO projetile nodes
        // CreatureInstance inst = inst.getParent
        // CreatureNode creaNode = this.get...
        // ProjectileNode projNode = GD.Load<PackedScene>(inst.model.meshScenePath).Instantiate<ProjectileNode>();
        // projNode.init(inst);
        // Ajoute le proj à la créature? pour on death ça clear au complet
        // creaNode.addChild(projNode)
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveProjectile(SmartSet<Projectile> list, Projectile inst)
    {

    }

    private void clearNodes()
    {
        // Cleanup old nodes
        this.Entities.QueueFreeChildren();
    }

}
