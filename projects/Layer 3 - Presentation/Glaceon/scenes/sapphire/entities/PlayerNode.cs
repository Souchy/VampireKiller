using Godot;
using Godot.Collections;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using System.Reflection.Emit;
using Util.communication.commands;
using Util.communication.events;
using vampierkiller.logia;
using vampirekiller.logia.commands;

public partial class PlayerNode : CreatureNode
{

    [NodePath]
    public Camera3D PlayerCamera { get; set; }
    [NodePath]
    public UiSapphire UiSapphire { get; set; }


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private Sapphire _game;
	private bool isCamLocked = true;

    [Inject]
	public ICommandPublisher publisher { get; set; }

	public override void _Ready()
	{
		base._Ready();
		this.OnReady();
		this.Inject();
		_game = (Sapphire) this.GetParent().GetParent();
        if(this.IsMultiplayerAuthority())
        {
            PlayerCamera.MakeCurrent();
        } else
        {
            this.UiSapphire.Hide();
        }
    }


	public override void _PhysicsProcess(double delta)
	{
        // TODO multiplayer authority, but also shouldn't block local serverless play
        // this.SetMultiplayerAuthority(1);		// put this omewhere else in the spawner
        //if (Universe.isOnline && !Multiplayer.IsServer())
        //    return;
        // here, control physics access. chaque joueur est autoritaire de son PlayerNode, les enemy ont l'autorité du serveur ou du joueur local
        if (Universe.isOnline && !this.IsMultiplayerAuthority()) 
            return;

        if (Input.IsActionJustPressed("lock_camera"))
        {
            isCamLocked = !isCamLocked;
            GD.Print("Cam locked: " + isCamLocked);
            this.PlayerCamera.TopLevel = !isCamLocked;
        }

        Vector3 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
		// If input, set velocity
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
			// stop the point & click navigation
			NavigationAgent3D.TargetPosition = GlobalPosition;
		}
		else
		// If point & click, set velocity
		if (physicsNavigationProcess(delta))
		{
			return;
		}
		else
		// If no input, slow down 
		if (NavigationAgent3D.IsNavigationFinished())
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		Vector3 fowardPoint = this.Position + velocity * 1;
        Vector3 lookAtTarget = new Vector3(fowardPoint.X, 0, fowardPoint.Z);
		betterLookAt(lookAtTarget);
        MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
    {
        // todo control authority
        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;

        base._Input(@event);

		Vector3 raycast = Vector3.Zero;

		bool clicked = Input.IsActionJustPressed("click_move") || Input.IsActionPressed("click_move");
		if (clicked)
		{
			if (raycast == Vector3.Zero)
				raycast = getRayCast();
			NavigationAgent3D.TargetPosition = raycast;
		}

        var playerId = this.GetMultiplayerAuthority();
		bool casted1 = Input.IsActionJustPressed("cast_slot_1");
		if (casted1)
		{
			if (raycast == Vector3.Zero)
				raycast = getRayCast();
			var cmd = new CommandCast(playerId, raycast, 0);
            this.publisher.publish(cmd);
		}
		bool casted2 = Input.IsActionJustPressed("cast_slot_2");
		if (casted2)
		{
			if (raycast == Vector3.Zero)
				raycast = getRayCast();
			var cmd = new CommandCast(playerId, raycast, 1);
			this.publisher.publish(cmd);
		}
		bool clear_projs = Input.IsActionJustPressed("clear_projs");
		if (clear_projs)
		{
			Universe.fight.projectiles.clear();
		}
	}

	private Vector3 getRayCast()
	{
		var mousePos = this.GetViewport().GetMousePosition();
		var rayLength = 100;
		var from = PlayerCamera.ProjectRayOrigin(mousePos);
		var to = from + PlayerCamera.ProjectRayNormal(mousePos) * rayLength;
		var space = GetWorld3D().DirectSpaceState;
		var ray = new PhysicsRayQueryParameters3D()
		{
			From = from,
			To = to,
			CollideWithAreas = true
		};
		var result = space.IntersectRay(ray);
		if (result.ContainsKey("position")){
			Vector3 pos = (Vector3)result["position"];
			pos.Y = 0;
			EventBus.centralBus.publish(nameof(UiSapphire.onRaycast), pos);
			return pos;
		}
		return Vector3.Zero;
	}

}
