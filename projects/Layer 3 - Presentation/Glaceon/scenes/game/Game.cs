using Godot;
using Godot.Sharp.Extras;
using Util.communication.events;
using Util.entity;
using Util.structures;
using VampireKiller.eevee;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee;

public partial class Game : Node
{
    [NodePath]
    public Node Players { get; set; }

    [NodePath]
    public Node Creatures { get; set; }
    
    [NodePath("%Camera3D")]
    public Camera3D camera3D { get; set; }

    private Fight fight; // Only kept around so we can unsubscribe when fight ends
    private bool isActivated = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        //Main.fight.creatures.GetEntityBus().subscribe(this);
        //Main.fight.projectiles.GetEntityBus().subscribe(this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void startFight(Fight newFight)
    {
        // Cleanup if fight is currently active
        if (this.fight != null)
        {
            this.stopFight();
        }

        this.fight = newFight;
        this.fight.creatures.GetEntityBus().subscribe(this);
        this.fight.projectiles.GetEntityBus().subscribe(this);

        // Instantiate already existing creatures
        foreach (var creature in this.fight.creatures.values)
        {
            this.onAddCreatureInstance(this.fight.creatures, creature);
        }
    }

    public void stopFight()
    {
        // Unsuscribe
        this.fight.creatures.GetEntityBus().unsubscribe(this);
        this.fight.projectiles.GetEntityBus().unsubscribe(this);
        this.fight = null;

        // Cleanup old nodes
        Game.clearChildren(this.Creatures);
        //Game.clearChildren(this.Projectiles);
    }

    [Subscribe(nameof(SmartSet<CreatureInstance>.add))]
    public void onAddCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        CreatureNode node = GD.Load<PackedScene>(inst.model.meshScenePath).Instantiate<CreatureNode>();
        inst.GetEntityBus().subscribe(node);
        node.init(inst);
        Creatures.AddChild(node);
        if (node is EnemyNode)
        {
            EnemyNode enemyNode = (EnemyNode) node;
            enemyNode.setTrackingTarget((Node3D) this.Players.GetChild(0));
        }
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveCreatureInstance(SmartSet<CreatureInstance> list, CreatureInstance inst)
    {
        // faut r�f�rence � creaInst.entityUid dans CreatureNode
    }

    [Subscribe(nameof(SmartSet<Projectile>.add))]
    public void onAddProjectile(SmartSet<Projectile> list, Projectile inst)
    {
        // TODO projetile nodes
        // CreatureInstance inst = inst.getParent
        // CreatureNode creaNode = this.get...
        // ProjectileNode projNode = GD.Load<PackedScene>(inst.model.meshScenePath).Instantiate<ProjectileNode>();
        // projNode.init(inst);
        // Ajoute le proj � la cr�ature? pour on death �a clear au complet
        // creaNode.addChild(projNode)
    }
    [Subscribe(nameof(SmartSet<CreatureInstance>.remove))]
    public void onRemoveProjectile(SmartSet<Projectile> list, Projectile inst)
    {

    }

    private static void clearChildren(Node node)
    {
        foreach (var item in node.GetChildren())
        {
            node.RemoveChild(item);
            item.QueueFree();
        }
    }
}
