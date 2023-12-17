using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.commands;
using vampierkiller.logia;
using vampirekiller.logia.commands;

public partial class PlayerNode : CreatureNode
{
	[NodePath]
	public SpringArm3D SpringArm3D { get; set; }

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private Game _game;
	private Camera3D _gameCamera;
	[Inject]
	public ICommandPublisher publisher { get; set; }

	public override void _Ready()
	{
		base._Ready();
		this.OnReady();
		this.Inject();
		_game = (Game)this.GetParent().GetParent();
		_gameCamera = this.GetViewport().GetCamera3D();
	}

	private bool isCamLocked = false;

	public override void _PhysicsProcess(double delta)
	{
		// TODO multiplayer authority, but also shouldn't block local serverless play
		// this.SetMultiplayerAuthority(1);		// put this omewhere else in the spawner
		// if(!this.IsMultiplayerAuthority())	// here, control physics access. chaque joueur est autoritaire de son PlayerNode, les enemy ont l'autorité du serveur ou du joueur local
		// 	return;

		if (Input.IsActionJustPressed("lock_camera"))
		{
			isCamLocked = !isCamLocked;
			GD.Print("Cam locked: " + isCamLocked);
			if (isCamLocked) {
				_game.Environment.RemoveChild(_gameCamera);
				this.SpringArm3D.AddChild(_gameCamera);
			} else {
				this.SpringArm3D.RemoveChild(_gameCamera);
				_game.Environment.AddChild(_gameCamera);
			}
		}



		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("move_jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
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
		// if (!NavigationAgent3D.IsNavigationFinished())
		// {
		// 	var nextPos = NavigationAgent3D.GetNextPathPosition();
		// 	direction = GlobalPosition.DirectionTo(nextPos);
		// 	// direction.Y = 0;
		// 	velocity = direction * Speed;
		// }
		else
		// If no input, slow down 
		if (NavigationAgent3D.IsNavigationFinished())
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		// todo control authority
		// if(!this.IsMultiplayerAuthority())
		// 	return;
		base._Input(@event);
		bool clicked = Input.IsActionJustPressed("click_move") || Input.IsActionPressed("click_move");
		if (clicked)
		{
			var mousePos = this.GetViewport().GetMousePosition();
			var rayLength = 100;
			var from = _gameCamera.ProjectRayOrigin(mousePos);
			var to = from + _gameCamera.ProjectRayNormal(mousePos) * rayLength;
			var space = GetWorld3D().DirectSpaceState;
			var ray = new PhysicsRayQueryParameters3D()
			{
				From = from,
				To = to,
				CollideWithAreas = true
			};
			var result = space.IntersectRay(ray);
			if (result.ContainsKey("position"))
				NavigationAgent3D.TargetPosition = (Vector3)result["position"];
		}

		bool casted = Input.IsActionJustPressed("cast_slot_1");
		if (casted)
		{
			var cmd = new CommandCast(this.creatureInstance, new Vector3(1, 0, 0));
			this.publisher.publish(cmd);
		}
	}

}
