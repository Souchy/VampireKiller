using Godot;
using System;
using VampireKiller.eevee.creature;


/// <summary>
/// Properties that need to be shown:
/// - life, lifeMax
/// - mana, manaMax MAYBE
/// - position, direction, speed (transform)
/// </summary>
public partial class CreatureNode : CharacterBody3D
{

	public override void _PhysicsProcess(double delta)
	{
        //this.Position
	}

    public void init(CreatureInstance inst)
    {
        inst.positionHook = () => this.Position;
    }


}
