using Godot;
using Godot.Sharp.Extras;
using System;

public partial class FxNode : Node3D
{
	[NodePath]
	public AnimationPlayer animationPlayer { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.OnReady();
		animationPlayer.AnimationFinished += onFinish;
		animationPlayer.Play("play");
	}

	/// <summary>
	/// When the animation is finished
	/// </summary>
	private void onFinish(StringName animName) {
		this.QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
