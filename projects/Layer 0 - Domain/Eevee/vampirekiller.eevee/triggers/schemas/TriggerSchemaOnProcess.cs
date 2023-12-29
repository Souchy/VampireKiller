
namespace vampirekiller.eevee.triggers.schemas;


public class TriggerSchemaOnProcess : ITriggerSchema
{
    public TriggerType triggerType => TriggerType.onProcess;
    // pourrait avoir des propriétés à check

    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
