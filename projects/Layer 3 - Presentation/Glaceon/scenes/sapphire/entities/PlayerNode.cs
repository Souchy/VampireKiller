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
using vampirekiller.glaceon.util;
using vampirekiller.logia.commands;
using vampirekiller.logia.net;

public partial class PlayerNode : CreatureNode
{

    private readonly string[] directionInputs = new string[] { "move_left", "move_right", "move_up", "move_down" };
    private readonly string[] castSlotInputs = new string[] { "cast_slot_0", "cast_slot_1", "cast_slot_2", "cast_slot_3" };

    [NodePath]
    public Camera3D PlayerCamera { get; set; }
    [NodePath]
    public UiSapphire UiSapphire { get; set; }

	private Sapphire _game;
	private bool isCamLocked = true;
	private int CAMERA_MAX_ZOOM = 25;
	private int CAMERA_MIN_ZOOM = 3;

    [Inject]
	public ICommandPublisher publisher { get; set; }

	public override void _Ready()
	{
		base._Ready();
		//this.OnReady();
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
    /// Direction for the next frame. Either from directionInputs or point & click
    /// </summary>
    protected override Vector3 getNextDirection() {
		var direction = getNextInputDirection();
		if (direction != Vector3.Zero)
			return direction;
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
        base._Input(@event);
        // todo control authority
        if (Universe.isOnline && !this.IsMultiplayerAuthority())
            return;

        (CreatureNode? raycastEntity, Vector3? raycastPosition)? raycast = null;

        // Directional inputs
        if (isAnyActionPressed(directionInputs))
        {
            // stop the point & click navigation
            NavigationAgent3D.TargetPosition = GlobalPosition;
        }

        // Point & Click inputs
        bool clicked = Input.IsActionJustPressed("click_move") || Input.IsActionPressed("click_move");
        if (clicked)
        {
            if (raycast == null)
                raycast = PlayerCamera.getRayCast();
            if (raycast.Value.raycastPosition != null)
                NavigationAgent3D.TargetPosition = (Vector3) raycast.Value.raycastPosition;
        }

        // Cast inputs
        for(int i = 0; i < castSlotInputs.Length; i++)
        {
            if (Input.IsActionJustPressed(castSlotInputs[i]))
                castCommand(i);
        }

        // Camera inputs
        inputsCamera();

        // Other inputs
        inputsOther();

        // Local method to hate read-write access to the same raycast
        void castCommand(int slot)
        {
            if (raycast == null)
                raycast = PlayerCamera.getRayCast();
            var skill = creatureInstance.activeSkills.getAt(slot);
            if(skill == null)
            {
                return;
            }
            var cmd = new CommandCast(creatureInstance, raycast.Value.raycastEntity?.creatureInstance, (Vector3) raycast.Value.raycastPosition, skill);
            this.publisher.publish(cmd);
            //var cmd = new CommandCast(playerId, raycast.Value.raycastEntity?.creatureInstance, (Vector3) raycast.Value.raycastPosition, slot);
            //this.playAttack(() => this.publisher.publish(cmd));
        }
	}

    private void inputsOther()
    {
		bool clear_projs = Input.IsActionJustPressed("clear_projs");
		if (clear_projs)
        {
            Universe.fight?.projectiles.clear();
		}
    }

    /// <summary>
    /// TODO: zoom en restant a -60 degre en x
    /// </summary>
    private void inputsCamera()
    {
        if (Input.IsActionJustPressed("lock_camera"))
        {
            isCamLocked = !isCamLocked;
            this.PlayerCamera.TopLevel = !isCamLocked;
            if (isCamLocked)
                this.PlayerCamera.Position = new Vector3(0, 8, 3);
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


}
