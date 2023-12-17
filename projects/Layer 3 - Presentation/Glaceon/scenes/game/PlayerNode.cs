using Godot;
using Godot.Sharp.Extras;
using System;

public partial class PlayerNode : CreatureNode
{
	private Camera3D _gameCamera;


	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	
    public override void _Ready()
    {
        this.OnReady();
		_gameCamera = this.GetViewport().GetCamera3D();
    }

	public override void _PhysicsProcess(double delta)
	{
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
		if(physicsNavigationProcess(delta)) 
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
			if(result.ContainsKey("position")) 
				NavigationAgent3D.TargetPosition = (Vector3)result["position"];
		}

	}

}
