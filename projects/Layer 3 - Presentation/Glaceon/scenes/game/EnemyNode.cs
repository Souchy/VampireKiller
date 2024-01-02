using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.commands;
using Util.communication.events;
using vampierkiller.logia;
using vampirekiller.eevee.ai;
using vampirekiller.eevee.creature;
using vampirekiller.logia.commands;

public partial class EnemyNode : CreatureNode
{

    [Inject]
    public ICommandPublisher publisher { get; set; }

    [NodePath]
    private Area3D AreaOfAttack;

    private Node3D trackingTarget;

    private int playersInAOE = 0;

    public override void _Ready()
    {
        base._Ready();
        this.Inject();
        AreaOfAttack.BodyEntered += this.onBodyEnterAreaOfAttack;
        AreaOfAttack.BodyExited += this.onBodyExitAreaOfAttack;
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

        if (playersInAOE != 0)
        {
            var cmd = new CommandCast(this.creatureInstance, this.Transform.Basis.Z, 0); //-this.Transform.Basis.Z, 1);
            this.attack(() => this.publisher.publish(cmd));
        }
    }

    private void onBodyEnterAreaOfAttack(Node3D body)
    {
        if (body is PlayerNode)
        {
            playersInAOE += 1;
        }
    }

    private void onBodyExitAreaOfAttack(Node3D body)
    {
        if (body is PlayerNode)
        {
            playersInAOE -= 1;
        }
    }

}
