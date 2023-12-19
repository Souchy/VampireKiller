using vampirekiller.eevee.triggers;

namespace Namespace;

public record TriggerEventOnStatement() : TriggerEvent(TriggerType.onStatement);

public class TriggerSchemaOnStatement : ITriggerSchema
{
    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
