
using Godot;
using VampireKiller;
using VampireKiller.scenes;

public partial class CreatureNode : Node3D {
    public CreatureInstance creature { get; set; }
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}