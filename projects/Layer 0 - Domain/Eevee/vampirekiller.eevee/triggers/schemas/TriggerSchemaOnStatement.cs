using vampirekiller.eevee.triggers;

namespace Namespace;

public class TriggerSchemaOnStatement : ITriggerSchema
{
    public TriggerType triggerType => TriggerType.onStatement;
    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
