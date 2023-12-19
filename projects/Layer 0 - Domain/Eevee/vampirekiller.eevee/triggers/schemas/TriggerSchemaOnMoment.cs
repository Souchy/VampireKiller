
using vampirekiller.eevee.enums;

namespace vampirekiller.eevee.triggers.schemas;


public record TriggerEventTimeline(MomentType moment) : TriggerEvent(TriggerType.onTime);

public class TriggerSchemaOnMoment : ITriggerSchema
{
    // pourrait avoir des propriétés à check
    
    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
