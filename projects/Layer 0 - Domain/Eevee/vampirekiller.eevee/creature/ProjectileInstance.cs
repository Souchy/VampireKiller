
using Godot;
using Util.ecs;
using Util.entity;
using VampireKiller.eevee.creature;

namespace VampireKiller.eevee;

public class ProjectileInstance : Entity, Identifiable
{
    // public ID entityUid { get; set; }

    public Vector3 direction {  get; private set; }
    public CreatureInstance originator { get; set; } // For retrieving spawn location & to avoid collisions with caster as the projectile spawns

    // Temp: below fields should be moved to a model class
    public float speed { get; set; }
    public string meshScenePath { get; set; }
    public int dmg {  get; set; }

    public ProjectileInstance() { }

    public void init(CreatureInstance originator, Vector3 direction, float speed, string meshScenePath, int dmg)
    {
        this.originator = originator;
        this.direction = direction;
        this.speed = speed;
        this.meshScenePath = meshScenePath;
        this.dmg = dmg;
    }



    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

public class BoardEntity
{
    /*
    // position 2d
    // dir 2d // orientation 1d
    // speed 1d
    PhysicsStats {}
    Scene gfxModel;
    Area3d body;
    */
}

public class PhysicsStats
{

}


/*
How can we have:
- Entity.position: vector3
- EntityScene.position: vector3
And keep it synced
Makes no sense does it
Physics loop will update the position based on acceleration, speed & orientation


Server runs physics
Client sends direction arrows inputs
Server changes the speed/direction based on these inputs
Physics updates the position
Server sends all positions periodically (1s/60)

It's also a PITA to convert Vector2d from the domain to Godot's vectors

*/
