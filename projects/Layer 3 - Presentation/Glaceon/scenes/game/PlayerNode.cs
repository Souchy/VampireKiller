using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.commands;
using vampierkiller.logia;
using vampirekiller.logia.commands;

public partial class PlayerNode : CreatureNode
{
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private Game _game;
	private Camera3D _gameCamera;
    private bool isCamLocked = true;
	private int CAMERA_Z_OFFSET = 5;
	private int CAMERA_MAX_ZOOM = 25;
	private int CAMERA_MIN_ZOOM = 8;
	[Inject]
	public ICommandPublisher publisher { get; set; }

	public override void _Ready()
	{
		base._Ready();
		this.OnReady();
		this.Inject();
		_game = (Game)this.GetParent().GetParent();
		_gameCamera = this.GetViewport().GetCamera3D();
		_gameCamera.Current = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		// TODO multiplayer authority, but also shouldn't block local serverless play
		// this.SetMultiplayerAuthority(1);		// put this omewhere else in the spawner
		// if(!this.IsMultiplayerAuthority())	// here, control physics access. chaque joueur est autoritaire de son PlayerNode, les enemy ont l'autorité du serveur ou du joueur local
		// 	return;

		Vector2 inputDir2D = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (!inputDir2D.IsZeroApprox()) this.NavigationAgent3D.TargetPosition = this.Position;
		Vector3 inputDir = new Vector3(inputDir2D.X, 0, inputDir2D.Y);
		Vector3 navDir = this.getNavigationVector();
		this.walk(inputDir.IsZeroApprox() ? navDir : inputDir);
		base._PhysicsProcess(delta);

		// Have character face in the mouse's direction
		var mousePos = this.GetViewport().GetMousePosition();
		var from = this._gameCamera.ProjectRayOrigin(mousePos);
		var to = from + this._gameCamera.ProjectRayNormal(mousePos) * 20;
		to.Y = this.Position.Y;
		this.LookAt(to);

		// Have game camera follow player
		if (this.isCamLocked)
		{
			this._gameCamera.Position = new Vector3(
				this.Position.X, 
				this._gameCamera.Position.Y,
				this.Position.Z + CAMERA_Z_OFFSET
			);
		}
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

		bool jumped = Input.IsActionJustPressed("move_jump") || Input.IsActionPressed("move_jump");
		if (jumped)
		{
			this.jump();
		}

		bool casted = Input.IsActionJustPressed("cast_slot_1");
		if (casted)
		{
			var cmd = new CommandCast(this.creatureInstance, -this.Transform.Basis.Z);
			this.publisher.publish(cmd);
		}

		bool zoomed_in = Input.IsActionJustPressed("zoom_in");
		if (zoomed_in && this._gameCamera.Position.Y > CAMERA_MIN_ZOOM)
			this._gameCamera.Position += new Vector3(0, -1, 0);
		bool zoomed_out = Input.IsActionJustPressed("zoom_out");
		if (zoomed_out && this._gameCamera.Position.Y < CAMERA_MAX_ZOOM)
			this._gameCamera.Position += new Vector3(0, 1, 0);
		bool toggle_cam_lock = Input.IsActionJustPressed("lock_camera");
		if (toggle_cam_lock)
			this.isCamLocked = !this.isCamLocked;
	}

}
