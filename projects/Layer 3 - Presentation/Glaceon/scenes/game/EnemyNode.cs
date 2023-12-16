using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using VampireKiller.eevee.creature;

public partial class EnemyNode : CreatureNode
{
    public const float Speed = 3.0f;
    
    [NodePath("NavigationAgent3D")]
    public NavigationAgent3D navAgent;

    private Node3D trackingTarget;
    
    public override void _Ready() 
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (this.trackingTarget != null) 
            this.navAgent.TargetPosition = this.trackingTarget.GlobalPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!this.navAgent.IsNavigationFinished())
        {
            var nextPathPosition = this.navAgent.GetNextPathPosition();
            var new_velocity = Speed * this.GlobalPosition.DirectionTo(nextPathPosition);
            
            this.Velocity = new_velocity;
            this.MoveAndSlide();
        }
    }
    
    public void setTrackingTarget(Node3D trackingTarget)
    {
        this.trackingTarget = trackingTarget;
        this.navAgent.TargetPosition = this.trackingTarget.GlobalPosition;
    }
}
