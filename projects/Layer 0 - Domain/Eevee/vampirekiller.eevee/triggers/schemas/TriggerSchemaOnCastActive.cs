
using vampirekiller.eevee.actions;
using vampirekiller.eevee.enums;

namespace vampirekiller.eevee.triggers.schemas;

public class TriggerSchemaOnCastActive: ITriggerSchema
{
    public TriggerType triggerType => TriggerType.onCastActive;
    // rien à check, accepte toutes les actions
    
    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
