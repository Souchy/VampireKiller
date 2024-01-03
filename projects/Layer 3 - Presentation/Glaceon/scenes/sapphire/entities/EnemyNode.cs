using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using Util.communication.events;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.ai;
using vampirekiller.eevee.creature;
using vampirekiller.logia.commands;
using vampirekiller.logia.extensions;

public partial class EnemyNode : CreatureNode
{

    [NodePath]
    private Area3D AreaOfAttack;

    private Node3D trackingTarget;

    private List<CreatureNode> playersInRange = new();

    public override void _Ready()
    {
        base._Ready();
        Speed = 3.0f;
        AreaOfAttack.BodyEntered += this.onBodyEnterAreaOfAttack;
        AreaOfAttack.BodyExited += this.onBodyExitAreaOfAttack;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Universe.isOnline && !this.Multiplayer.IsServer()) 
            return;

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

        if (playersInRange.Count > 0)
        {
            // TODO select player & active in AI
            var selectedPlayer = playersInRange.First();
            var selectedActive = 0; // cast le premier skill.
            
            // TODO: CommandCast pour mobs (pas de playerid)
            var action = new ActionCastActive() {
                sourceEntity = this.creatureInstance.entityUid, //command.source.entityUid,
                raycastEntity = selectedPlayer.creatureInstance.entityUid,
                raycastPosition = selectedPlayer.creatureInstance.position,
                fight = Universe.fight,
                slot = selectedActive,
            };
            if (action.canApplyCast()) {
                this.playAttack(action.applyActionCast);
            }
            // var cmd = new CommandCast(this.creatureInstance, this.Transform.Basis.Z, 0); //-this.Transform.Basis.Z, 1);
            // this.attack(() => this.publisher.publish(cmd));
        }
    }
    
	protected override Vector3 getNextDirection() {
		return getNextNavigationDirection();
	}

    // public override void _PhysicsProcess(double delta)
    // {
    //     //base._PhysicsProcess(delta);
    //     if (Universe.isOnline && !this.Multiplayer.IsServer()) //!this.IsMultiplayerAuthority())
    //         return;
    //     getNextNavigationDirection(delta);
    // }
    
    private void onBodyEnterAreaOfAttack(Node3D body)
    {
        if (body is PlayerNode p)
        {
            if(playersInRange.Count == 0) {
                trackingTarget = p;
            }
            playersInRange.Add(p);
        }
    }

    private void onBodyExitAreaOfAttack(Node3D body)
    {
        if (body is PlayerNode p)
        {
            playersInRange.Remove(p);
        }
    }


}
