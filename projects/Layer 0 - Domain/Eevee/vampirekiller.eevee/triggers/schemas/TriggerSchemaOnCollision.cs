
using Util.ecs;
using vampirekiller.eevee.enums;

namespace vampirekiller.eevee.triggers.schemas;

public class TriggerSchemaOnCollision : ITriggerSchema
{
    public TriggerType triggerType => TriggerType.onCollision;
    // pourrait avoir des propriétés à check sur l'event
    // ex: quel type de collision ? 
    // pas le type d'entité pcq c'est déjà dans le TriggerListener.holderConditions + triggererConditions
    
    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
