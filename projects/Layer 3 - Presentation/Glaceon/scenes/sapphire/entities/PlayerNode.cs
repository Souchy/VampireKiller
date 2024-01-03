using Godot;
using Godot.Collections;
using Godot.Sharp.Extras;
using Logia.vampirekiller.logia;
using System;
using System.Reflection.Emit;
using Util.communication.commands;
using Util.communication.events;
using vampierkiller.logia;
using vampirekiller.glaceon.autoload;
using vampirekiller.logia.commands;
using vampirekiller.logia.net;

public partial class PlayerNode : CreatureNode
{

	private readonly string[] directionInput = new string[] { "move_left", "move_right", "move_up", "move_down" };

    [NodePath]
    public Camera3D PlayerCamera { get; set; }
    [NodePath]
    public UiSapphire UiSapphire { get; set; }

	private Sapphire _game;
	private bool isCamLocked = true;
	private int CAMERA_MAX_ZOOM = 25;
	private int CAMERA_MIN_ZOOM = 8;

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

	/// <summary>
	/// Calculate next direction based on inputs (directionInputs + point & click)
	/// </summary>
	protected override Vector3 getNextDirection() {
		var direction = getNextInputDirection();
		// If input, set velocity
		if (direction != Vector3.Zero)
		{
			return direction;
		}
		return getNextNavigationDirection();
	}

	private Vector3 getNextInputDirection() {
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
		return direction;
	}


	public bool isAnyActionPressed(params string[] actions)
	{
		foreach (var name in actions)
		{
			if (Input.IsActionPressed(name))
				return true;
		}
		return false;
	}

	public override void _Input(InputEvent @event)
    {
        // todo control authority
        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;

        base._Input(@event);
		
		if(isAnyActionPressed(directionInput)){
			// stop the point & click navigation
			NavigationAgent3D.TargetPosition = GlobalPosition;
		}

        (CreatureNode? raycastEntity, Vector3? raycastPosition)? raycast = null;

        bool clicked = Input.IsActionJustPressed("click_move") || Input.IsActionPressed("click_move");
        if (clicked)
        {
            if (raycast == null)
                raycast = getRayCast();
            if (raycast.Value.raycastPosition != null)
                NavigationAgent3D.TargetPosition = (Vector3) raycast.Value.raycastPosition;
        }

        var playerId = this.GetMultiplayerAuthority();
		bool casted1 = Input.IsActionJustPressed("cast_slot_1");
		if (casted1)
		{
			if (raycast == null)
				raycast = getRayCast();
			var cmd = new CommandCast(playerId, raycast.Value.raycastEntity?.creatureInstance, (Vector3) raycast.Value.raycastPosition, 0);
            this.playAttack(() => this.publisher.publish(cmd));
		}
		bool casted2 = Input.IsActionJustPressed("cast_slot_2");
		if (casted2)
		{
			if (raycast == null)
				raycast = getRayCast();
			var cmd = new CommandCast(playerId, raycast.Value.raycastEntity?.creatureInstance, (Vector3) raycast.Value.raycastPosition, 1);
			this.playAttack(() => this.publisher.publish(cmd));
		}
		bool clear_projs = Input.IsActionJustPressed("clear_projs");
		if (clear_projs)
        {
            Universe.fight?.projectiles.clear();
		}
        if (Input.IsActionJustPressed("lock_camera"))
        {
            isCamLocked = !isCamLocked;
            GD.Print("Cam locked: " + isCamLocked);
            this.PlayerCamera.TopLevel = !isCamLocked;
			this.PlayerCamera.Position = Vector3.Zero;
        }
		bool zoomed_in = Input.IsActionJustPressed("zoom_in");
		if (zoomed_in && this.PlayerCamera.Position.Y > CAMERA_MIN_ZOOM) {
			this.PlayerCamera.Position += new Vector3(0, -1, 0);
		}
		bool zoomed_out = Input.IsActionJustPressed("zoom_out");
		if (zoomed_out && this.PlayerCamera.Position.Y < CAMERA_MAX_ZOOM) {
			this.PlayerCamera.Position += new Vector3(0, 1, 0);
		}
	}

	private (CreatureNode?, Vector3?) getRayCast()
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
        CreatureNode? raycastEntity = null;
        Vector3? raycastPosition = null;
        if (result.ContainsKey("collider"))
        {
            // Different colliders: ground (StaticBody3D), creature (CreatureNode).
            // Theorical: poe corpses (spectres), totems (prob a creature), maybe some bullshit doors or destructible decor  
            Node3D collider = (Node3D) result["collider"];
            if(collider is CreatureNode crea)
                raycastEntity = crea;
        }
		if (result.ContainsKey("position")) {
			Vector3 pos = (Vector3) result["position"];
			pos.Y = 0;
            raycastPosition = pos;
        }
        EventBus.centralBus.publish(nameof(UiSapphire.onRaycast), raycastEntity, raycastPosition);
        return (raycastEntity, raycastPosition);
		//return Vector3.Zero;
	}


}
