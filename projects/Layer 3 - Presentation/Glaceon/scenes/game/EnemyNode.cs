using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using vampirekiller.eevee.ai;
using vampirekiller.eevee.creature;

public partial class EnemyNode : CreatureNode
{

    private Node3D trackingTarget;

    public override void _Ready()
    {
        base._Ready();
        Speed = 3.0f;
    }

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
        }
        if (this.trackingTarget != null)
            this.NavigationAgent3D.TargetPosition = this.trackingTarget.GlobalPosition;
    }

}
