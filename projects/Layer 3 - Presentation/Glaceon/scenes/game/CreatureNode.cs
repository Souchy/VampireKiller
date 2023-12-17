using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.stats;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;


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

    public float Speed = 5.0f;
    public float JumpVelocity = 6.0f;

    public override void _Ready()
    {
        this.OnReady();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        physicsNavigationProcess(delta);
    }

    protected bool physicsNavigationProcess(double delta)
    {
        // If point & click, set velocity
        if (!NavigationAgent3D.IsNavigationFinished())
        {
            var nextPos = NavigationAgent3D.GetNextPathPosition();
            var direction = GlobalPosition.DirectionTo(nextPos);
            Velocity = direction * Speed;
            try {
                if(!Position.IsEqualApprox(nextPos))
                    this.MeshInstance3D.LookAt(nextPos);
            } catch(Exception e) {}
            MoveAndSlide();
            return true;
        }
        return false;
    }


    public void init(CreatureInstance crea)
    {
        creatureInstance = crea;
        creatureInstance.set(this);
        creatureInstance.getPositionHook = () => this.GlobalPosition;
        creatureInstance.setPositionHook = (Vector3 v) => this.GlobalPosition = v;
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        if (creatureInstance != null)
            this.GlobalPosition = creatureInstance.spawnPosition;
    }


    [Subscribe]
    public void onStatChanged(CreatureInstance crea, IStat stat)
    {
        // todo regrouper les life stats en une liste<type> automatique genre / avoir une annotation [Life] p.ex, etc
        if (stat is CreatureAddedLife || stat is CreatureAddedLifeMax || stat is CreatureBaseLife || stat is CreatureBaseLifeMax || stat is CreatureIncreaseLife || stat is CreatureIncreaseLifeMax)
        {
            var life = crea.getTotalStat<CreatureTotalLife>();
            var max = crea.getTotalStat<CreatureTotalLifeMax>();
            Healthbar.Value = life.value / max.value;
        }
    }

    [Subscribe]
    public void onItemListAdd(object list, object item)
    {
        // check all statements 
        //      modify mesh / material / etc si nécessaire
    }
    [Subscribe]
    public void onItemListRemove(object list, object item)
    {

    }
    [Subscribe]
    public void onStatusListAdd(object list, object item)
    {

    }
    [Subscribe]
    public void onStatusListRemove(object list, object item)
    {

    }

}
