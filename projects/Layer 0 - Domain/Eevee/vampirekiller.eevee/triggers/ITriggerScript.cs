using vampirekiller.eevee.actions;

namespace vampirekiller.eevee.triggers;

public interface ITriggerScript
{
    public Type schemaType { get; }

    /// <summary>
    /// Returns true if the trigger should proc the listener & apply the statement associated.
    /// </summary>
    /// <param name="action">Can be any kind of action</param>
    /// <param name="trigger"></param>
    /// <param name="schema">The TriggerListener's schema</param>
    public bool checkTrigger(IAction action, TriggerEvent trigger, ITriggerSchema schema); //(ActionTrigger action, TriggerListener listener); // 
}
