
using Godot;
using Util.ecs;
using Util.entity;
using Util.structures;
using vampirekiller.eevee.enums;
using vampirekiller.eevee.stats.schemas.skill;
using VampireKiller.eevee.creature;
using VampireKiller.eevee.vampirekiller.eevee.spells;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace VampireKiller.eevee;

public class ProjectileInstance : Entity, Identifiable, IStatementContainer
{
    // public ID entityUid { get; set; }
    public EntityGroupType creatureGroup { get => get<EntityGroupType>(); set => set<EntityGroupType>(value); }
    public CreatureInstance source { get; set; } // For retrieving spawn location & to avoid collisions with caster as the projectile spawns
    public SmartList<IStatement> statements { get; set; } = SmartList<IStatement>.Create();


    public string meshScenePath { get; set; }
    public Vector3 spawnPosition { get; set; }
    // public Vector3 position { get => get<Vector3>(); }
    public Vector3 spawnDirection { get; private set; }
    // Temp: below fields should be moved to a model class
    public double spawnSpeed { get; set; }
    //public StatsDic stats { get; set; } = Register.Create<StatsDic>();
    public DateTime? expirationDate { get; set; } = null;
    public int remainingReturnCount { get; set; } = 0;


    private ProjectileInstance() { }

    public static ProjectileInstance create() {
        var proj = new ProjectileInstance();
        return proj;
    }
    public void init(CreatureInstance originator, Vector3 direction, double speed, string meshScenePath)
    {
        this.source = originator;
        this.spawnDirection = direction;
        this.spawnSpeed = speed;
        this.meshScenePath = meshScenePath;
    }



    public void Dispose()
    {
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
