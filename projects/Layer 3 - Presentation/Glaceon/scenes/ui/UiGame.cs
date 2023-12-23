using Godot;
using Godot.Sharp.Extras;
using System;

public partial class UiGame : Control
{
	[NodePath]
	public Label LblFps { get; set; }
	public UiSlotActive slot { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.OnReady();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var fps = Engine.GetFramesPerSecond();
		LblFps.Text = "fps: " + fps.ToString();
	}
}
