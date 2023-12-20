
using VampireKiller.eevee.vampirekiller.eevee.statements;

namespace vampirekiller.eevee.statements.schemas;

public class SpawnProjectileSchema : IStatementSchema
{
    /// <summary>
    /// Scene to spawn
    /// </summary>
    public string scene { get; set; }
    /// <summary>
    /// Statements to give to the ProjectileInstance so it can proc them onCollision, onExpire, etc
    /// </summary>
    public List<IStatement> children { get; set; } = new();

    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
