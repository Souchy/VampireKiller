using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.commands;
using vampierkiller.logia;
using vampirekiller.logia.commands;
using VampireKiller.eevee;

public partial class ProjectileNode : Area3D
{
	[Inject]
	public ICommandPublisher publisher { get; set; }

	public ProjectileInstance projectileInstance { get; private set; }
	private Vector3 velocity = new Vector3(); 

	public override void _Ready()
	{
		base._Ready();
		this.OnReady();
		this.Inject();
		this.BodyEntered += this.onBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (this.projectileInstance != null)
		{
			Transform3D transform = this.Transform;
			transform.Origin += this.velocity * (float) delta;
			this.Transform = transform;
		}
	}

	public void init(ProjectileInstance projectileInstance)
	{
		this.projectileInstance = projectileInstance;
		this.velocity = this.projectileInstance.direction * this.projectileInstance.speed;
		this.GlobalPosition = this.projectileInstance.originator.position;
	}

	private void onBodyEntered(Node3D body)
	{
		if (body is CreatureNode)
		{
			CreatureNode collider = (CreatureNode) body;
			// Avoid collisions with originator to let the projectile spawn
			if (collider.creatureInstance != this.projectileInstance.originator)
			{
				CommandProjectileCollision commandProjectileCollision = new CommandProjectileCollision(this.projectileInstance, collider.creatureInstance);
				this.publisher.publish(commandProjectileCollision);
			}
		}
	}
}
