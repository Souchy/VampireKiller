using Godot;
using Godot.Sharp.Extras;
using System;

public partial class UiResourcePopup : Node3D
{
    [NodePath]
    public Label3D Label3D { get; set; }
    [NodePath]
    public AnimationPlayer AnimationPlayer { get; set; }

    public int value = 0;
    private static Color red = new Color("c72a34");
    private static Color green = new Color("20c726");
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.OnReady();
        if(value >= 0)
            this.Label3D.Modulate = green;
        else
            this.Label3D.Modulate = red;
        this.Label3D.Text = value.ToString();
        AnimationPlayer.AnimationFinished += onFinish;
        AnimationPlayer.Play("play");
    }

    public override void _EnterTree()
    {
        base._EnterTree();
    }

    private void onFinish(StringName animName)
    {
        this.QueueFree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
