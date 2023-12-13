
namespace VampireKiller.eevee;

public class Projectile
{

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
