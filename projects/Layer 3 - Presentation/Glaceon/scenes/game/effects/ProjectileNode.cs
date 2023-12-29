using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.commands;
using Util.entity;
using vampierkiller.logia;
using vampirekiller.logia.commands;
using VampireKiller.eevee;
using vampirekiller.logia.extensions;
using Logia.vampirekiller.logia;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.triggers.schemas;

public partial class ProjectileNode : Area3D
{
	[Inject]
	public ICommandPublisher publisher { get; set; }

	public ProjectileInstance projectileInstance { get; private set; }
	private Vector3 velocity = new Vector3();
	private double expiry = 15; // seconds
	private double timeAlive = 0;

	public override void _Ready()
	{
		base._Ready();
		this.OnReady();
		this.Inject();
		this.BodyEntered += this.onBodyEntered;
		this.velocity = projectileInstance.spawnDirection * (float) projectileInstance.spawnSpeed;
		this.GlobalPosition = projectileInstance.spawnPosition; //projectileInstance.source.position;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (this.projectileInstance != null)
		{
			Transform3D transform = this.Transform;
			transform.Origin += this.velocity * (float) delta;
			this.Transform = transform;
			this.timeAlive += delta;
		}

		if (timeAlive > expiry)
		{
			// temporaire
			Universe.fight.projectiles.remove(this.projectileInstance);
        }
	}

	public void init(ProjectileInstance proj)
	{
		projectileInstance = proj;
		projectileInstance.set<ProjectileNode>(this);
		projectileInstance.GetEntityBus().subscribe(this);
		projectileInstance.set<Func<Vector3>>(() => this.GlobalPosition);
	}

	private void onBodyEntered(Node3D body)
	{
		if(this.projectileInstance == null)
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
				projectileInstance.procTriggers(action);
				// temporaire
				this.QueueFree();
			}
		}
	}
}
