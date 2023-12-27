
using VampireKiller.eevee.vampirekiller.eevee.enums;

namespace vampirekiller.eevee.statements.schemas;

public class SpawnFxSchema : IStatementSchema
{

    public const string EventFx = "fx";

    /// <summary>
    /// Scene to spawn
    /// </summary>
    public string scene { get; set; }
    public bool follow { get; set; } = false;
    public ActorType followActor { get; set; } = ActorType.Target;

    public IStatementSchema copy()
    {
        throw new NotImplementedException();
    }
}
