using Godot;
using Godot.Sharp.Extras;
using System;
using Util.communication.events;
using VampireKiller.eevee.creature;


/// <summary>
/// Properties that need to be shown:
/// - life, lifeMax
/// - mana, manaMax MAYBE
/// - position, direction, speed (transform)
/// </summary>
public partial class CreatureNode : CharacterBody3D
{

    //[NodePath]
    //public Node3D Model3d { get; set; }
    //[NodePath]
    //public AnimationPlayer player { get; set; }

    public override void _Ready() 
    {
        base._Ready();
        this.OnReady();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {

    }

    public void init(CreatureInstance inst)
    {
        inst.getPositionHook = () => this.Position;
        inst.setPositionHook = (Vector3 v) => this.Position = v;
    }

    [Subscribe]
    public void onItemListAdd(object list, object item)
    {
        // check all statements 
        //      modify mesh / material / etc si n�cessaire
    }
    [Subscribe]
    public void onItemListRemove(object list, object item)
    {

    }
    [Subscribe]
    public void onStatusListAdd(object list, object item)
    {

    }
    [Subscribe]
    public void onStatusListRemove(object list, object item)
    {

    }

}
