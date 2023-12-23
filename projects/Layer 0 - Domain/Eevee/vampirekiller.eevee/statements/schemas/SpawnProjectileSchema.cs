
using Godot;
using Util.entity;
using VampireKiller.eevee.vampirekiller.eevee.statements;
using VampireKiller.eevee.vampirekiller.eevee.stats;

namespace vampirekiller.eevee.statements.schemas;

public class SpawnProjectileSchema : IStatementSchema
{
    /// <summary>
    /// True: add the node to the Game's Entities node.
    /// False: add the node to the caster's CreatureNode.
    /// </summary>
    public bool parentToEntities { get; set; } = true;
    /// <summary>
    /// 
    /// </summary>
    public float spawnOffset { get; set; }
    /// <summary>
    /// Scene to spawn
    /// </summary>
    public string scene { get; set; }
    /// <summary>
    /// Contains Projectile stats like: proj count, chains, fireInCircle...
    /// </summary>
    public StatsDic stats { get; set; } = Register.Create<StatsDic>();

    /// <summary>
    /// Statements to give to the ProjectileInstance so it can proc them onCollision, onExpire, etc
    /// </summary>
    public List<IStatement> children { get; set; } = new();

    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
