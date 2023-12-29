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


    [Export]
    private double jumpWindupTimeInSeconds= 0.7;
    [Export]
    private double attackWindupTimeInSeconds = 0;
    
    // [NodePath]
    // public Node3D Model3d { get; set; }
    [NodePath]
    public AnimationPlayer AnimationPlayer { get; set; }

    [NodePath]
    public NavigationAgent3D NavigationAgent3D { get; set; }

    [NodePath("SubViewport/UiResourceBars")]
    public MarginContainer UiResourceBars { get; set; }
    [NodePath("SubViewport/UiResourceBars/VBoxContainer/Healthbar")]
    public ProgressBar Healthbar { get; set; }

    public float Speed = 5.0f;
    public float JumpVelocity = 6.0f;

    private CreatureNodeAnimationController animationController;

    public override void _Ready()
    {
        this.OnReady();
        this.animationController = new CreatureNodeAnimationController(this.AnimationPlayer, this.jumpWindupTimeInSeconds, this.attackWindupTimeInSeconds);
        
        // GD.Print(this.Name + " ready");
        if (creatureInstance != null)
        {
            // this.GlobalPosition = creatureInstance.spawnPosition;
            updateHPBar();
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

        // Set idle animation if not moving
        if (this.Velocity.IsZeroApprox())
        {
            this.animationController.playAnimation(CreatureNodeAnimationController.SupportedAnimation.Idle);
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
        this.animationController.playAnimation(CreatureNodeAnimationController.SupportedAnimation.Walk);
    }

    protected void jump()
    {
        Action windupCallback = () =>
        {
            var velocity = this.Velocity;
            velocity.Y = JumpVelocity;
            this.Velocity = velocity;
        };
        this.animationController.playAnimation(CreatureNodeAnimationController.SupportedAnimation.Jump, windupCallback);
    }

    protected void attack()
    {
        this.animationController.playAnimation(CreatureNodeAnimationController.SupportedAnimation.Attack);
    }

    protected void death()
    {
        this.animationController.playAnimation(CreatureNodeAnimationController.SupportedAnimation.Death);
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


public class CreatureNodeAnimationController
{
    // Defined in the order of priorities
    // if the player is jumping, keep animating jump if walk input is received
    // if the player is walking, cancel the walk animation and start jumping if jump input is received
    public enum SupportedAnimation
    {
        Idle,   // Loop animation
        Walk,   // Loop animation
        Jump,   // Action animation
        Attack, // Action animation
        Death,  // Action animation
        Unknown // Used as a fallback
    }

    private AnimationPlayer player;
    private double jumpWindupTimeInSeconds;
    private double attackWindupTimeInSeconds;

    private System.Collections.Generic.Dictionary<SupportedAnimation, string> animationToAnimationName;
    private SupportedAnimation currentAnimation = SupportedAnimation.Idle;

    public CreatureNodeAnimationController(AnimationPlayer player, double jumpWindupTimeInSeconds, double attackWindupTimeInSeconds)
    {
        this.player = player;
        this.jumpWindupTimeInSeconds = jumpWindupTimeInSeconds;
        this.attackWindupTimeInSeconds = attackWindupTimeInSeconds;
        this.animationToAnimationName = initAnimations(player);
        this.playAnimation(SupportedAnimation.Idle);
    }

    public bool playAnimation(SupportedAnimation animation)
    {
        // Make sure animation exists
        if (!this.animationToAnimationName.ContainsKey(animation))
        {
            return false;
        }

        // If animation playing, make sure we are in a state that allows us to override it
        if (this.player.IsPlaying())
        {
            // If the animation is the same as current one, do not override
            if (this.currentAnimation == animation)
            {
                return false;
            }
            // If the animation has lower priority than the current one, and the current animation is not looping, do not override
            // (looping animations need to be able to override eachother to avoid needing to wait until the end of the loop to change animation)
            if (this.currentAnimation > animation && !isLoopingAnimation(this.currentAnimation))
            {
                return false;
            }
        }

        var animationName = animationToAnimationName.GetValue(animation);
        this.currentAnimation = animation;
        this.player.Play(animationName);
        return true;
    }

    public bool playAnimation(SupportedAnimation animation, Action onWindupEnd)
    {
        bool animationPlayed = this.playAnimation(animation);
        if (animationPlayed)
        {
            this.executeWindupCallback(animation, onWindupEnd);
        }
        return animationPlayed;
    }

    private async void executeWindupCallback(SupportedAnimation animation, Action onWindupEnd)
    {
        double windupTimeInSeconds = getWindupTimeInSeconds(animation);
        await Task.Delay((int) (windupTimeInSeconds * 1000));
        onWindupEnd();
    }

    private double getWindupTimeInSeconds(SupportedAnimation animation)
    {
        switch (animation)
        {
            case SupportedAnimation.Jump:
                return this.jumpWindupTimeInSeconds;
            case SupportedAnimation.Attack:
                return this.attackWindupTimeInSeconds;
            default:
                return this.player.GetAnimation(this.animationToAnimationName[animation]).Length;
        }
    }

    private static bool isLoopingAnimation(SupportedAnimation animation)
    {
        return animation <= SupportedAnimation.Walk;
    }

    private static System.Collections.Generic.Dictionary<SupportedAnimation, string> initAnimations(AnimationPlayer player)
    {
        var result = new System.Collections.Generic.Dictionary<SupportedAnimation, string>();
        foreach (var animationName in player.GetAnimationList())
        {
            result.Add(matchAnimation(animationName), animationName);
        }
        return result;
    }

    private static SupportedAnimation matchAnimation(String animationName)
    {
        var lowerCaseAnimationName = animationName.ToLower();
        foreach (var supportedAnimation in Enum.GetValues<SupportedAnimation>())
        {
            if (lowerCaseAnimationName.Contains(supportedAnimation.ToString().ToLower())) 
            {
                return supportedAnimation;
            }
        }
        return SupportedAnimation.Unknown;
    }
}
