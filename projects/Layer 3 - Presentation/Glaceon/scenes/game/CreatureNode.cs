using Godot;
using Godot.Collections;
using Godot.Sharp.Extras;
using GodotSharpKit.Generator;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Util.communication.events;
using Util.entity;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.stats;
using VampireKiller.eevee.vampirekiller.eevee.stats.schemas;
using vampirekiller.logia.extensions;


/// <summary>
/// Properties that need to be shown:
/// - life, lifeMax
/// - mana, manaMax MAYBE
/// - position, direction, speed (transform)
/// </summary>
public partial class CreatureNode : CharacterBody3D
{
    public CreatureInstance creatureInstance;
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        
    // [NodePath]
    // public Node3D Model3d { get; set; }
    [NodePath]
    public CreatureNodeAnimationPlayer CreatureNodeAnimationPlayer { get; set; }

    [NodePath]
    public NavigationAgent3D NavigationAgent3D { get; set; }

    [NodePath("SubViewport/UiResourceBars")]
    public MarginContainer UiResourceBars { get; set; }
    [NodePath("SubViewport/UiResourceBars/VBoxContainer/Healthbar")]
    public ProgressBar Healthbar { get; set; }

    [NodePath]
    public VariantController VariantController;

    public float Speed = 5.0f;

    public override void _Ready()
    {
        this.OnReady();
        
        // GD.Print(this.Name + " ready");
        if (creatureInstance != null)
        {
            // this.GlobalPosition = creatureInstance.spawnPosition;
            updateHPBar();
            VariantController.updateVariant(this.creatureInstance.model.meshSceneVariant);
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        // Add the gravity.
        if (!IsOnFloor())
        {
            Vector3 velocity = this.Velocity;
            velocity.Y -= gravity * (float)delta;
            this.Velocity = velocity;
        }

        MoveAndSlide();
    }

    protected Vector3 getNavigationVector()
    {
        if (!NavigationAgent3D.IsNavigationFinished())
        {
            var nextPos = NavigationAgent3D.GetNextPathPosition();
            return GlobalPosition.DirectionTo(nextPos);
        }
        return new Vector3();
    }

    protected void walk(Vector3 direction)
    {
        var velocity = this.Velocity;
        velocity.X = direction.X * Speed;
        velocity.Z = direction.Z * Speed;
        this.Velocity = velocity;
        if (this.Velocity.IsZeroApprox())
        {
            this.CreatureNodeAnimationPlayer.playAnimation(CreatureNodeAnimationPlayer.SupportedAnimation.Idle);
        } else
        {
            this.CreatureNodeAnimationPlayer.playAnimation(CreatureNodeAnimationPlayer.SupportedAnimation.Walk);
        }
    }

    protected void attack(Action attackCallback)
    {
        this.CreatureNodeAnimationPlayer.playAnimation(CreatureNodeAnimationPlayer.SupportedAnimation.Attack, attackCallback);
    }

    protected void death()
    {
        this.CreatureNodeAnimationPlayer.playAnimation(CreatureNodeAnimationPlayer.SupportedAnimation.Death);
    }

    public void init(CreatureInstance crea)
    {
        // GD.Print(this.Name + " init");
        creatureInstance = crea;
        creatureInstance.set<CreatureNode>(this);
        creatureInstance.GetEntityBus().subscribe(this);
        creatureInstance.getPositionHook = () => this.GlobalPosition;
        creatureInstance.setPositionHook = (Vector3 v) => this.GlobalPosition = v;
        creatureInstance.set<Func<Vector3>>(() => this.GlobalPosition);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        // GD.Print(this.Name + " enter tree");
        if (creatureInstance != null)
        {
            this.GlobalPosition = creatureInstance.spawnPosition;
        }
    }


    [Subscribe(CreatureInstance.EventUpdateStats)]
    public void onStatChanged(CreatureInstance crea, IStat stat)
    {
        // GD.Print("CreatureNode: onStatChanged: " + stat.GetType().Name + " = " + stat.genericValue);
        // todo regrouper les life stats en une liste<type> automatique genre / avoir une annotation [Life] p.ex, etc
        if (stat is CreatureAddedLife || stat is CreatureAddedLifeMax || stat is CreatureBaseLife || stat is CreatureBaseLifeMax || stat is CreatureIncreaseLife || stat is CreatureIncreaseLifeMax)
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
    // [Subscribe]
    // public void onStatusListRemove(object list, object item)
    // {

    // }

private void updateHPBar()
    {
        var life = this.creatureInstance.getTotalStat<CreatureTotalLife>();
        var max = this.creatureInstance.getTotalStat<CreatureTotalLifeMax>();
        double value = ((double) life.value / (double) max.value) * 100;
        // GD.Print("Crea (" + this.Name + ") update hp %: " + value); // + "............" + Healthbar + " vs " + hpbar);
        Healthbar.Value = value;
    }

}


