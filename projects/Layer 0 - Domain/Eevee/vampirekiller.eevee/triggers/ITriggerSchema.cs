namespace vampirekiller.eevee.triggers;

public interface ITriggerSchema
{
    public TriggerType triggerType { get; }
    public ITriggerSchema copy();
}
