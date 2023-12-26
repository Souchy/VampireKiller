using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
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
                var player = creaInstance.get<CreatureNode>();
                this.trackingTarget = player;
            }
        }
        if (this.trackingTarget != null)
            this.NavigationAgent3D.TargetPosition = this.trackingTarget.GlobalPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        //base._PhysicsProcess(delta);
        if (Universe.isOnline && !this.Multiplayer.IsServer()) //!this.IsMultiplayerAuthority())
            return;
        physicsNavigationProcess(delta);
    }

}
