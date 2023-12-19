using vampirekiller.eevee.actions;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.triggers.schemas;

namespace vampirekiller.logia.triggers;

public class TriggerScriptOnCollision : ITriggerScript
{
    public Type schemaType => typeof(TriggerEventOnCollision);

    public bool checkTrigger(IAction action, TriggerEvent trigger, ITriggerSchema schema)
    {
        throw new NotImplementedException();
    }

}
