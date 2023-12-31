using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using vampirekiller.eevee.ai;
using vampirekiller.eevee.creature;

public partial class EnemyNode : CreatureNode
{
    [NodePath]
    private Area3D AreaOfAttack;

    private Node3D trackingTarget;

    public override void _Ready()
    {
        base._Ready();
        AreaOfAttack.BodyEntered += this.onBodyEnterAreaOfAttack;
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
        Vector3 direction = this.getNavigationVector();
        this.walk(direction);
        base._PhysicsProcess(delta);

        Vector3 lookAtTarget = new Vector3(
            this.NavigationAgent3D.TargetPosition.X,
            this.Position.Y,
            this.NavigationAgent3D.TargetPosition.Z
        );
        if (!lookAtTarget.IsEqualApprox(this.Position))
            this.LookAt(lookAtTarget);
    }

    private void onBodyEnterAreaOfAttack(Node3D body)
    {
        if (body is PlayerNode)
        {
            this.attack(() => { });
        }
    }

}
