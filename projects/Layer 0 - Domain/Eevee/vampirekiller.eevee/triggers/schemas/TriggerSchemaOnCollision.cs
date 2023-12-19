
using vampirekiller.eevee.enums;

namespace vampirekiller.eevee.triggers.schemas;

public record TriggerEventOnCollision(
    // contient sûrement une Entity qui peut collide (creature, projectile...)
) : TriggerEvent(TriggerType.onCollision);

public class TriggerSchemaOnCollision : ITriggerSchema
{
    // pourrait avoir des propriétés à check sur l'event
    // ex: quel type de collision ? 
    // pas le type d'entité pcq c'est déjà dans le TriggerListener.holderConditions + triggererConditions
    
    public ITriggerSchema copy()
    {
        throw new NotImplementedException();
    }
}
