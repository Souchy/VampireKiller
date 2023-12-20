
namespace vampirekiller.eevee.statements.schemas;

public class SpawnFxSchema : IStatementSchema
{
    public string scene { get; set; }

    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
