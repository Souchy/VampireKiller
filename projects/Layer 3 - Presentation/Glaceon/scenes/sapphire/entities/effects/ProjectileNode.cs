using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.commands;
using Util.entity;
using vampierkiller.logia;
using vampirekiller.logia.commands;
using VampireKiller.eevee;
using vampirekiller.logia.extensions;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.triggers.schemas;
using Logia.vampirekiller.logia;
using vampirekiller.eevee.stats.schemas.skill;
using Eevee.vampirekiller.eevee.stats.schemas;

/// <summary>
/// TODO: remove any projectile that goes outside the process range.
/// There's a render box (the screen)
/// And a process box (slightly larger)
/// When a projectile goees out the process box, check if it has the ProjectileReturn stat.
/// If it can return, return. Otherwise remove the projInstance from the fight.
/// Also check if the projectile has a SkillExpirationDate stat and remove the projInstance if it's passed.
/// </summary>
public partial class ProjectileNode : Area3D
{
    /// <summary>
    /// Range at which things stop to process and are removed from the fight.
    /// todo: put this somewhere else
    /// </summary>
    public const float outOfBoundrange = 50;

    [Inject]
    public ICommandPublisher publisher { get; set; }

    public ProjectileInstance projectileInstance { get; private set; }
    private Vector3 velocity = new Vector3();

    public override void _Ready()
    {
        base._Ready();
        this.OnReady();
        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;
        this.Inject();
        this.BodyEntered += this.onBodyEntered;
        this.velocity = projectileInstance.spawnDirection * (float) projectileInstance.spawnSpeed;
        this.GlobalPosition = projectileInstance.spawnPosition;
    }

    public override void _PhysicsProcess(double delta)
    {

        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;
        if (this.projectileInstance != null)
        {
            Transform3D transform = this.Transform;
            transform.Origin += this.velocity * (float) delta;
            this.Transform = transform;


            if (projectileInstance.expirationDate != null && projectileInstance.expirationDate < DateTime.Now)
            {
                expire();
            }
            else
            if(this.GlobalPosition.Length() > outOfBoundrange)
            {
                if(projectileInstance.remainingReturnCount > 0)
                {
                    // set direction back to the creature source
                    projectileInstance.remainingReturnCount--;
                    var dir = projectileInstance.source.position - this.GlobalPosition;
                    dir.Y = 0;
                    dir = dir.Normalized();
                    this.velocity = dir * (float) projectileInstance.spawnSpeed;
                } 
                else
                {
                    expire();
                }
            }

        }
    }

    public void init(ProjectileInstance proj)
    {
        projectileInstance = proj;
        projectileInstance.set<ProjectileNode>(this);
        projectileInstance.GetEntityBus().subscribe(this);
        projectileInstance.set<Func<Vector3>>(() =>
        {
            //try
            //{
                return this.GlobalPosition;
            //} catch(Exception e) { }
            //return Vector3.Zero;
        });
    }

     TEST TDOTDO ka sdklajm SD
    public override void _ExitTree()
    {
        base._ExitTree();
        projectileInstance.remove<ProjectileNode>();
        projectileInstance.remove<Func<Vector3>>();
    }

    private void onBodyEntered(Node3D body)
    {
        if (this.projectileInstance == null)
            return;
        // GD.Print("Proj collision with: " + body);
        if (body is CreatureNode)
        {
            CreatureNode collider = (CreatureNode) body;
            // Avoid collisions with originator to let the projectile spawn
            if (collider.creatureInstance != this.projectileInstance.source)
            {
                // GD.Print("Collision with a creature other than caster: " + collider);
                // Action proc tous les listeners onCollision
                var action = new ActionCollision(projectileInstance, collider.creatureInstance);
                action.setContextProjectile(projectileInstance);
                projectileInstance.procTriggers(action);

                expire();
            }
        }
    }

    private void expire()
    {
        // 
        Universe.fight.projectiles.remove(this.projectileInstance);
        //this.QueueFree();
    }

}
