using Godot;
using Logia.vampirekiller.logia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract partial class CreatureNode : CharacterBody3D
{

    protected void betterLookAt(Vector3 nextPos)
    {
        // check is to avoid following warning: Up vector and direction between node origin and target are aligned, look_at() failed
        if (!GlobalPosition.IsEqualApprox(nextPos) && !Vector3.Up.Cross(nextPos - this.GlobalPosition).IsZeroApprox())
        {
            Model.LookAt(nextPos);
            Model.RotateY(Mathf.Pi);
        }
    }

    /// <summary>
    /// Get the next frame's direction
    /// </summary>
    /// <returns></returns>
    protected abstract Vector3 getNextDirection();

    /// <summary>
    /// Get the next frame's direction from the navigation agent, or Zero if no current path.
    /// </summary>
    /// <returns></returns>
    protected Vector3 getNextNavigationDirection()
    {
        // If point & click, set velocity
        if (!NavigationAgent3D.IsNavigationFinished())
        {
            var nextPos = NavigationAgent3D.GetNextPathPosition();
            nextPos.Y = 0;
            var direction = GlobalPosition.DirectionTo(nextPos);
            direction.Y = 0;
            return direction.Normalized();
        }
        return Vector3.Zero;
    }

    /// <summary>
    /// Delta is in seconds
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;

        refreshCache(delta);

        var direction = getNextDirection();
        var speed = cachedTotalMovementSpeed;

        var velocity = this.Velocity;
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        // If no input, slow down 
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
        }
        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y -= gravity * (float) delta;
        }

        // Set velocity. // TODO: Root motion: velocity based on animation's root bone movement
        this.Velocity = velocity;
        MoveAndSlide();

        // Look at
        if(Velocity.Length() > 0)
        {
            Vector3 fowardPoint = this.GlobalPosition + Velocity * 1;
            Vector3 lookAtTarget = new Vector3(fowardPoint.X, 0, fowardPoint.Z);
            betterLookAt(lookAtTarget);
        }

        // Animation idle/walk
        if(creatureInstance == null)
            return;
        var velo = this.Velocity.Length();
        SetAnimationFromVelocity(velo);
    }

    protected virtual void SetAnimationFromVelocity(float velo)
    {
        if (velo <= 0.001)
        {
            this.CreatureNodeAnimationPlayer.playAnimationLoop(AnimationState.idle, this.creatureInstance.currentSkin.animations.idle);
        }
        else
        if (velo >= 1 && velo < 2)
        {
            this.CreatureNodeAnimationPlayer.playAnimationLoop(AnimationState.moving, this.creatureInstance.currentSkin.animations.walk, cachedIncreasedMovementSpeed);
        }
        else
        if (velo >= 2)
        {
            this.CreatureNodeAnimationPlayer.playAnimationLoop(AnimationState.moving, this.creatureInstance.currentSkin.animations.run, cachedIncreasedMovementSpeed);
        }
    }

}
