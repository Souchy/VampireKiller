
namespace vampirekiller.eevee.triggers.schemas;


public record TriggerEventProcessTick(double delta) : TriggerEvent(TriggerType.onProcess);

public class TriggerSchemaOnProcess : ITriggerSchema
{
    // pourrait avoir des propriétés à check

    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
