using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using vampirekiller.eevee.ai;
using vampirekiller.eevee.creature;

public partial class EnemyNode : CreatureNode
{
    public const float Speed = 3.0f;

    [NodePath("NavigationAgent3D")]
    public NavigationAgent3D navAgent;

    private Node3D trackingTarget;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (this.trackingTarget == null && creatureInstance != null)
        {
            var model = (EnemyModel) creatureInstance.model;
            var creaInstance = model.ai.findTarget();
            if(creaInstance != null) {
                var player = creaInstance.get<PlayerNode>();
                this.trackingTarget = player;
            }
            //var players = this.GetNode("%Players").GetChildren<PlayerNode>();
            //foreach(var player in players)
            //    if(player.creatureInstance == creaInstance)
            //        this.trackingTarget = player;
        }
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
