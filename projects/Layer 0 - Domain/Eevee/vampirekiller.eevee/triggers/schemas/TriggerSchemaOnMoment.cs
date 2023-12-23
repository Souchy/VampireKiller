
using vampirekiller.eevee.enums;

namespace vampirekiller.eevee.triggers.schemas;


public class TriggerSchemaOnMoment : ITriggerSchema
{
    public TriggerType triggerType => TriggerType.onTime;
    // pourrait avoir des propriétés à check
    
    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
