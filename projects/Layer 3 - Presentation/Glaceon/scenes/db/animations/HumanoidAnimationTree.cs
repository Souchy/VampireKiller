using Godot;
using System;

public partial class HumanoidAnimationTree : AnimationTree
{
    private CharacterBody3D character;
    public bool moving {  get; set; }
    public bool onFloor { get; set; }

    public override void _Ready()
    {
        this.character = GetParent<CharacterBody3D>();
    }

    public override void _Process(double delta) {
        if (this.character != null && this.character is CharacterBody3D)
        {
            this.Set("parameters/conditions/moving", !this.character.Velocity.IsZeroApprox());
            this.Set("parameters/conditions/onFloor", this.character.IsOnFloor());
            this.Set("parameters/conditions/notMoving", this.character.Velocity.IsZeroApprox());
            this.Set("parameters/conditions/notOnFloor", !this.character.IsOnFloor());
        }
    }
}
