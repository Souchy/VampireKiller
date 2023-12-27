using Namespace;
using Util.entity;
using vampirekiller.eevee.actions;
using vampirekiller.eevee.triggers;
using vampirekiller.eevee.triggers.schemas;

namespace vampirekiller.logia.triggers;

/// <summary>
/// Returns true only if the creatorStatemtn is null or equal to the action's statement that led to this
/// </summary>
public class TriggerScriptOnStatusAdd : ITriggerScript
{
    public Type schemaType => typeof(TriggerSchemaOnStatusAdd);

    public bool checkTrigger(IActionTrigger action, ITriggerSchema schema)
    {
        try
        {
            TriggerSchemaOnStatusAdd props = (TriggerSchemaOnStatusAdd) schema;
            if(props.spellModelIdFilter == null) //.creatorStatement == null)
                return true;

            var actionCast = action.getParent<ActionCastActive>();
            var actionTrigger = action.getParent<ActionTrigger>();
            ID? modelID = actionCast?.getActive()?.modelUid;
            if (modelID == null)
            {
                modelID = actionTrigger?.getContextProjectile()?.spellModelUid;
            }
            if (modelID == null)
            {
                modelID = actionTrigger?.getContextStatus()?.modelUid;
            }
            if(props.spellModelIdFilter == modelID)
            {
                return true;
            }
            //ActionStatementTarget createStatusAction = (ActionStatementTarget) action.parent;
            //if (createStatusAction.statement == props.creatorStatement)
            //    return true;
        }
        catch (Exception ex)
        {

        }
        return false;
    }
    
}
