
namespace vampirekiller.eevee.statements.schemas;

public class SpawnFxSchema : IStatementSchema
{
    /// <summary>
    /// Scene to spawn
    /// </summary>
    public string scene { get; set; }

    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
