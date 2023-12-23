using vampirekiller.eevee.actions;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.triggers.schemas;

namespace vampirekiller.logia.triggers;

public class TriggerScriptOnCollision : ITriggerScript
{
    public Type schemaType => typeof(TriggerSchemaOnCollision);

    public bool checkTrigger(IActionTrigger action, ITriggerSchema schema)
    {
        ActionCollision actionCollision = (ActionCollision) action;
        // quoi de plus à checker?
        return true;
    }
    
}
