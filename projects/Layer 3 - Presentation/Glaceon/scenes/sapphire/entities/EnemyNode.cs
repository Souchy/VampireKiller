using Godot;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using souchyutil.godot.animation;
using souchyutil.godot.components.multimesh;
using System;
using System.Collections.Generic;
using System.Linq;
using Util.communication.commands;
using Util.communication.events;
using vampierkiller.logia;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.ai;
using vampirekiller.eevee.creature;
using vampirekiller.glaceon.sapphire.entities;
using vampirekiller.logia.commands;
using vampirekiller.logia.extensions;

public partial class EnemyNode : CreatureNode
{

    [NodePath]
    private Area3D AreaOfAttack;

    private Node3D trackingTarget;

    private List<CreatureNode> playersInRange = new();
    public InstanceData InstanceData { get; set; }

    [Inject]
    public ICommandPublisher publisher { get; set; }

    public override void _Ready()
    {
        base._Ready();
        this.Inject();
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
            if (creaInstance != null)
            {
                var player = creaInstance.get<CreatureNode>();
                this.trackingTarget = player;
            }
        }

        if(true)
            return;

        if (this.trackingTarget != null)
            this.NavigationAgent3D.TargetPosition = this.trackingTarget.GlobalPosition;

        if (playersInRange.Count > 0)
        {
            // TODO select player & active in AI
            var selectedPlayer = playersInRange.First();
            var selectedSlot = 0; // cast le premier skill.

            var skill = creatureInstance.activeSkills.getAt(selectedSlot);
            var raycastEntity = selectedPlayer.creatureInstance;
            var raycastPosition = selectedPlayer.creatureInstance.position;

            var cmd = new CommandCast(creatureInstance, raycastEntity, raycastPosition, skill);
            this.publisher.publish(cmd);
            //this.playAttack(() => this.publisher.publish(cmd));

            // TODO: CommandCast pour mobs (pas de playerid)
            //var action = new ActionCastActive() {
            //    sourceEntity = this.creatureInstance.entityUid, //command.source.entityUid,
            //    raycastEntity = selectedPlayer.creatureInstance.entityUid,
            //    raycastPosition = selectedPlayer.creatureInstance.position,
            //    fight = Universe.fight,
            //    slot = selectedActive,
            //};
            //if (action.canApplyCast()) {
            //    this.playAttack(action.applyActionCast);
            //}
            //var cmd = new CommandCast(playerId, raycast.Value.raycastEntity?.creatureInstance, (Vector3) raycast.Value.raycastPosition, slot);
            // var cmd = new CommandCast(this.creatureInstance, this.Transform.Basis.Z, 0); //-this.Transform.Basis.Z, 1);
            // this.attack(() => this.publisher.publish(cmd));
        }

    }

    protected override void SetAnimationFromVelocity(float velo)
    {
        if (InstanceData != null)
        {
            InstanceData.Transform3D = Model.GlobalTransform;
            if(InstanceData.LoopAnimation != null)
                return;
            //var moveAnim = "";
            //if (velo <= 0.001)
            //{
            //    moveAnim = creatureInstance.currentSkin.animations.idle;
            //}
            //else if (velo < 2)
            //{
            //    moveAnim = creatureInstance.currentSkin.animations.walk;
            //}
            //else if (velo >= 2)
            //{
            //    moveAnim = creatureInstance.currentSkin.animations.run;
            //}
            var libId = 1; //int.Parse(moveAnim[..1]);
            var animId = 7; //int.Parse(moveAnim[2..]);


            var crowd = this.creatureInstance.get<CrowdNode>();
            var anim = crowd.AnimationHeaders[libId][animId];

            AnimationHeader toCurrent = anim;
            AnimationHeader toLoop = null;
            if (anim.LoopMode != Animation.LoopModeEnum.None)
                toLoop = anim;
            // if attacking -> set current headers.attack
            // if walking -> set current + set loop headers.walk
            // if dead -> set current = headers.dying
            //            set loop = headers.dead
            if (toCurrent != null && InstanceData.CurrentAnimation != toCurrent)
            {
                InstanceData.SetCurrentAnimation(toCurrent);
                //if (toShot.LoopMode == Animation.LoopModeEnum.None)
                //    InstanceData.SetCurrentAnimation(toShot);
                //else
                //    InstanceData.PlayLoopAnimation(toLoop);
            }
            if (toLoop != null && toLoop != InstanceData.LoopAnimation)
            {
                InstanceData.SetLoopAnimation(toLoop);
            }
        }
    }

    protected override Vector3 getNextDirection()
    {
        if(trackingTarget == null)
            return Vector3.Zero;
        return this.trackingTarget.GlobalPosition - this.GlobalPosition;
        //return getNextNavigationDirection();
    }

    private void onBodyEnterAreaOfAttack(Node3D body)
    {
        if (body is PlayerNode p)
        {
            if (playersInRange.Count == 0)
            {
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
