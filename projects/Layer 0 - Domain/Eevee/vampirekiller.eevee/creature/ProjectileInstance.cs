
using Godot;
using Util.ecs;
using Util.entity;
using Util.structures;
using vampirekiller.eevee.enums;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace VampireKiller.eevee;

public class ProjectileInstance : Entity, Identifiable, IStatementContainer
{
    // public ID entityUid { get; set; }
    public EntityGroupType creatureGroup { get => get<EntityGroupType>(); set => set<EntityGroupType>(value); }

    public CreatureInstance originator { get; set; } // For retrieving spawn location & to avoid collisions with caster as the projectile spawns
    public SmartList<IStatement> statements { get; set; } = SmartList<IStatement>.Create();
    
    public Vector3 direction {  get; private set; }
    // Temp: below fields should be moved to a model class
    public float speed { get; set; }
    public string meshScenePath { get; set; }
    public int dmg {  get; set; }


    private ProjectileInstance() { }

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
