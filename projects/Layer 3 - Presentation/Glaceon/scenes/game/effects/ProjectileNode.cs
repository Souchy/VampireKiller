using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.commands;
using Util.entity;
using vampierkiller.logia;
using vampirekiller.logia.commands;
using VampireKiller.eevee;

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
		this.velocity = projectileInstance.direction * projectileInstance.speed;
		this.GlobalPosition = projectileInstance.originator.position + new Vector3(0, 1, 0);
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
			CommandProjectileCollision commandProjectileCollision = new CommandProjectileCollision(this.projectileInstance, null);
			this.publisher.publish(commandProjectileCollision);
		}
	}

	public void init(ProjectileInstance proj)
	{
		projectileInstance = proj;
		projectileInstance.set<ProjectileNode>(this);
		projectileInstance.GetEntityBus().subscribe(this);
	}

	private void onBodyEntered(Node3D body)
	{
		// GD.Print("Proj collision with: " + body);
		if (body is CreatureNode)
		{
			CreatureNode collider = (CreatureNode) body;
			// Avoid collisions with originator to let the projectile spawn
			if (collider.creatureInstance != this.projectileInstance.originator)
			{
	        	// GD.Print("Collision with a creature other than caster: " + collider);
				CommandProjectileCollision commandProjectileCollision = new CommandProjectileCollision(this.projectileInstance, collider.creatureInstance);
				this.publisher.publish(commandProjectileCollision);
			}
		}
	}
}
