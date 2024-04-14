using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using Util.entity;
using vampirekiller.eevee.statements.schemas;

public partial class FxNode : Node3D
{
	[NodePath]
	public AnimationPlayer animationPlayer { get; set; }

    private Status? _status;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.OnReady();
        if (_status == null)
        {
            animationPlayer.AnimationFinished += onFinish;
        }
        animationPlayer.Play("play");
    }

    public void init(Status? status)
    {
        this._status = status;
        status?.GetEntityBus()?.subscribe(this);
    }

    [Subscribe(Status.EventRemove)]
    public void onStatusRemove(Status s)
    {
        _status?.GetEntityBus()?.unsubscribe(this);

        //if(this.IsInsideTree())
        //{
        //    var tree = this.GetTree();
        //    var root = tree.Root;
        //    var path = this.GetPath();
        //    var parent = this.GetParent();
        //    if (root.HasNode(path))
        //        this.QueueFree();
        //}

        try
        {
            // Free only if not freed already. Could be removed if the creature holder dies before the status ends.
            this.QueueFree();
        } catch(Exception e)
        {

        }
    }

	/// <summary>
	/// When the animation is finished
	/// </summary>
	private void onFinish(StringName animName) {
		this.QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(double delta)
	//{
	//}

}
