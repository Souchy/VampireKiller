
using Util.ecs;
using vampirekiller.eevee.enums;

namespace vampirekiller.eevee.triggers.schemas;

public class TriggerSchemaOnDeath : ITriggerSchema
{
    public TriggerType triggerType => TriggerType.onDeath;
    
    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
