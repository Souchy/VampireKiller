
using vampirekiller.eevee.actions;
using vampirekiller.eevee.enums;

namespace vampirekiller.eevee.triggers.schemas;

public record TriggerEventOnCastActive(ActionCastActive action) : TriggerEvent(TriggerType.onCastActive);

public class TriggerSchemaOnCastActive: ITriggerSchema
{
    // rien à check, accepte toutes les actions
    
    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
